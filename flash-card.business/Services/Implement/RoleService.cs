using Ardalis.Result;
using flash_card.business.Repository;
using flash_card.data.Entities;
using flash_card.data.Model.Request.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace flash_card.business.Services.Implement
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                var items = await _unitOfWork.RoleRepository.FindAsync(r => r.Status == false).ToListAsync();
                return new List<Role>(items);
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

            _unitOfWork.Dispose();
            return result;
        }

        public async Task<Role> UpdateRole(UpdateRoleRequest request)
        {
            var roleExist = _unitOfWork.RoleRepository.FindAsync(r => r.Id == request.Id).FirstOrDefault();
            if (roleExist == null) return Result<Role>.NotFound();
            try
            {
                roleExist.Name = request.Name;
                await _unitOfWork.RoleRepository.UpdateAsync(roleExist);
            }
            catch(Exception ex)
            {
                return Result<Role>.Error(new[] { ex.Message });
            }
            await _unitOfWork.CompleteAsync();
            _unitOfWork.Dispose();
            
            return roleExist;
        }

        public async Task<string> DeleteRole(int id)
        {
            var roleExist = _unitOfWork.RoleRepository.FindAsync(r => r.Id == id).FirstOrDefault();
            if (roleExist == null) return Result<string>.Error(new[] { "UserID is wrong or not in the system."  });
            try
            {
                roleExist.Status = false;
                await _unitOfWork.RoleRepository.UpdateAsync(roleExist);
            }
            catch (Exception ex)
            {
                return Result<string>.Error(new[] { ex.Message });
            }
            await _unitOfWork.CompleteAsync();
            _unitOfWork.Dispose();
            
            return Result<string>.Success("The role has been deleted.");
        }

    }
}
