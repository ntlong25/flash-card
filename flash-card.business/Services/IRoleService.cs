using flash_card.data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flash_card.business.Services
{
    public interface IRoleService
    {
        Task<List<Role>> GetAll();
        Task<Role> GetRole(int id);
        Task<Role> CreateRole(string roleName);
    }
}
