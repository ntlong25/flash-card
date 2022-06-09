using flash_card.business.Repository;
using flash_card.data.Entities;
using flash_card.data.Model;
using flash_card.data.Model.Response;
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
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result;
using flash_card.data.Model.Request;

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

        public async Task<List<User>> GetAll()
        {
            try
            {
                var items = await _unitOfWork.UserRepository.ListAsync();

                return new List<User>((IEnumerable<User>)items);
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<List<User>>.Error(new[] { ex.Message });
            }
        }
        public Task<User> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public int GetRoleIdByRoleName(string roleName)
        {
            var roleId = _unitOfWork.RoleRepository.FindAsync(role => role.Name.Equals(roleName)).SingleOrDefault().Id;
            return roleId;
        }
        public bool CheckPassword(User user, string password)
        {
            PasswordHasher<User> passHasher = new PasswordHasher<User>();
            PasswordVerificationResult result = passHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result.ToString().Equals("Success")) return true;
            return false;
        }

        public async Task<TokenResult> Login(string email, string password)
        {
            try
            {
                var user = _unitOfWork.UserRepository.FindAsync(user => user.Email.Equals(email)).SingleOrDefault() ?? null;
                var checkpass = CheckPassword(user, password);

                if (!checkpass || user == null)
                {
                    return Result<TokenResult>.NotFound();
                }
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Secret"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>() {
                        new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                        new Claim("Name", user.Name.ToString()),
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
                }, out SecurityToken validatedToken);
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
        public int GetUserIdJustAdded(string email)
        {
            try
            {
                var lastUserIdAdded = _unitOfWork.UserRepository.FindAsync(u => u.Email == email && u.Status == true).SingleOrDefault();
                return lastUserIdAdded.Id;
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<int>.Error(new[] { ex.Message });
            }
        }
        public async Task<User> CreateUser(CreateUserRequest request)
        {
            var existUser = _unitOfWork.UserRepository.FindAsync(user => user.Email.Equals(user.Email)).FirstOrDefault();
            if (existUser != null)  return existUser;
            var newUser = new User
            {
                UserName = request.Email,
                Name = request.Name,
                RoleId = GetRoleIdByRoleName(request.RoleName),
                Email = request.Email,
                PasswordHash = request.Password,
                PhoneNumber = "",
                DOB = DateTime.Now,
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
                var userAdd = await _unitOfWork.UserRepository.AddAsync(newUser);
            }
            catch (Exception ex)
            {
                return Result<User>.Error(new[] { ex.Message });
            }

            await _unitOfWork.CompleteAsync();
            return newUser;
        }
    }
}
