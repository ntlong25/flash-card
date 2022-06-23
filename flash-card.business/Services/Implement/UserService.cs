using flash_card.business.Repository;
using flash_card.data.Entities;
using flash_card.data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using flash_card.data.Model.Request.Users;

namespace flash_card.business.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
        }

        // return UsersResponse
        public async Task<List<User>> GetAll()
        {
            try
            {
                var items = await _unitOfWork.UserRepository
                    .FindAsync(user => user.Status==true)
                    .Include(r => r.Role)
                    .Include(t => t.Topics)
                    .ToListAsync();

                return new List<User>(items);
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<List<User>>.Error(new[] { ex.Message });
            }
        }
        // return UserResponse
        public async Task<User> GetUser(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository
                    .FindAsync(user => user.Status == true && user.Id == id)
                    .Include(r => r.Role)
                    .Include(t => t.Topics)
                    .FirstOrDefaultAsync();
                if (user == null || user.Status == false)
                {
                    return null;
                }
                return user;
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<User>.Error(new[] { ex.Message });
            }
        }

        public int GetRoleIdByRoleName(string roleName)
        {
            var role = _unitOfWork.RoleRepository
                .FindAsync(role => role.Name.Equals(roleName) && role.Status == true)
                .FirstOrDefault();
            if (role == null)
            {
                throw new Exception("Role not found");
            }
            return role.Id;
        }

        public bool CheckPassword(User user, string password)
        {
            try
            {
                PasswordHasher<User> passHasher = new PasswordHasher<User>();
                PasswordVerificationResult result = passHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (result.ToString().Equals("Success")) 
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<bool>.Error(new[] { ex.Message });
            }
            
        }

        public async Task<TokenResult> Login(string email, string password)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.FindAsync(user => user.Email.Equals(email) && user.Status == true).SingleOrDefaultAsync();
                //var user = await _userManager.FindByEmailAsync(email) ?? null;
                if (user == null)
                {
                    return Result<TokenResult>.Error("User not found");
                }
                var checkPass = CheckPassword(user, password);

                if (!checkPass)
                {
                    return Result<TokenResult>.Error("Invalid email or password");
                }
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Secret"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>() {
                        new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                        new Claim("Name", user.Name),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email)
                    };

                var role = _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId).Result.Name;

                claims.Add(new Claim("Role", role));
                

                var token = new JwtSecurityToken(
                    //payload
                    issuer: null,
                    audience: null,
                    claims,
                    expires: DateTime.Now.AddMinutes(120),
                    //header
                    signingCredentials: credentials);

                var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);
                return new TokenResult()
                {
                    Token = tokenResult,
                    User = new AccountModel()
                    {
                        Id = user.Id,
                        Role = role,
                        Name = user.Name
                    }
                };
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<TokenResult>.Error(new[] { ex.Message });
            }
        }

        public bool VerifyToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Secret"]));
            try
            {
                new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = securityKey
                }, out var validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public async Task<string> ChangePassword(string email, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? await _userManager.FindByNameAsync(email);
            var changePass = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (changePass.Succeeded)
            {
                return await Task.FromResult("Change success.");
            }
            return await Task.FromResult("Change failed.");
        }
        //public int GetUserIdJustAdded(string email)
        //{
        //    try
        //    {
        //        var lastUserIdAdded = _unitOfWork.UserRepository.FindAsync(u => u.Email == email && u.Status == true).SingleOrDefault();
        //        return lastUserIdAdded.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Log details here
        //        return Result<int>.Error(new[] { ex.Message });
        //    }
        //}
        public async Task<User> CreateUser(CreateUserRequest request)
        {
            var existUser = _unitOfWork.UserRepository.FindAsync(user => user.Email.Equals(request.Email)).FirstOrDefault();
            if (existUser != null)  return existUser;
            var newUser = new User
            {
                UserName = request.Email,
                Name = request.Name,
                RoleId = GetRoleIdByRoleName(request.RoleName),
                Email = request.Email,
                PasswordHash = request.Password,
                PhoneNumber = "",
                Dob = DateTime.Now,
                Address = "",
                CreateAt = DateTime.Now,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                Status = true
            };

            PasswordHasher<User> passHasher = new PasswordHasher<User>();
            newUser.PasswordHash = passHasher.HashPassword(newUser, newUser.PasswordHash);
            try
            {
                await _unitOfWork.UserRepository.AddAsync(newUser);
            }
            catch (Exception ex)
            {
                return Result<User>.Error(new[] { ex.Message });
            }

            await _unitOfWork.CompleteAsync();
            return newUser;
        }

        public async Task<User> UpdateUser(UpdateUserRequest request)
        {
            var existUser = _unitOfWork.UserRepository.FindAsync(user => user.Id.Equals(request.Id)).FirstOrDefault();
            if (existUser == null) return Result<User>.NotFound();
            existUser.Name = request.Name;
            existUser.Email = request.Email;
            existUser.PhoneNumber = request.PhoneNumber;
            existUser.Address = request.Address;

            PasswordHasher<User> passHasher = new PasswordHasher<User>();
            existUser.PasswordHash = passHasher.HashPassword(existUser, request.Password);
            try
            {
                await _unitOfWork.UserRepository.UpdateAsync(existUser);
            }
            catch (Exception ex)
            {
                return Result<User>.Error(new[] { ex.Message });
            }

            await _unitOfWork.CompleteAsync();
            return existUser;
        }

        public async Task<string> DeleteUser(int id)
        {
            var existUser = _unitOfWork.UserRepository.FindAsync(user => user.Id.Equals(id)).FirstOrDefault();
            if (existUser == null) return Result<string>.Error(new[] { "UserID is wrong or not in the system." });
            existUser.Status = false;
            try
            {
                await _unitOfWork.UserRepository.UpdateAsync(existUser);
            }
            catch (Exception ex)
            {
                return Result<string>.Error(new[] { ex.Message });
            }

            await _unitOfWork.CompleteAsync();
            return Result<string>.Success("The user has been deleted.");
        }
    }
}
