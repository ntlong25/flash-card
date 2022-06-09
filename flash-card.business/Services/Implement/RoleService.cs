using Ardalis.Result;
using flash_card.business.Repository;
using flash_card.data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Services.Implement
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public RoleService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Role> GetRole(int id)
        {
            try
            {
                return await _unitOfWork.RoleRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<Role>.Error(new[] { ex.Message });
            }
        }

        public async Task<List<Role>> GetAll()
        {
            try
            {
                var items = await _unitOfWork.RoleRepository.ListAsync();

                return new List<Role>((IEnumerable<Role>)items);
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<List<Role>>.Error(new[] { ex.Message });
            }

        }

        public async Task<Role> CreateRole(string roleName)
        {
            Role result;
            try
            {
                var item = new Role
                {
                    Name = roleName
                };
                result = await _unitOfWork.RoleRepository.AddAsync(item);
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<Role>.Error(new[] { ex.Message });
            }
            await _unitOfWork.CompleteAsync();
            
            return result;
        }

    }
}
