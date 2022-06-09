using flash_card.data.Entities;
using flash_card.data.Model;
using flash_card.data.Model.Request;
using flash_card.data.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<TokenResult> Login(string email, string password);
        int GetRoleIdByRoleName(string roleName);
        Task<User> GetUser(int id);
        Task<User> CreateUser(CreateUserRequest request);
        Task<string> ChangePassword(string email, string oldPassword, string newPassword);
        bool VerifyToken(string token);
    }
}
