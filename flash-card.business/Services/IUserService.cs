using flash_card.data.Entities;
using flash_card.data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using flash_card.data.Model.Request.Users;

namespace flash_card.business.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<TokenResult> Login(string email, string password);
        int GetRoleIdByRoleName(string roleName);
        Task<User> GetUser(int id);
        Task<string> ChangePassword(string email, string oldPassword, string newPassword);
        bool VerifyToken(string token);
        Task<User> CreateUser(CreateUserRequest request);
        Task<User> UpdateUser(UpdateUserRequest request);
        Task<string> DeleteUser(int id);
    }
}
