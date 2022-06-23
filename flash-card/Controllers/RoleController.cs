using flash_card.business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using flash_card.data.Model.Request.Roles;

namespace flash_card.api.Controllers
{
    [ApiController]
    [Route("/role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [AllowAnonymous]
        [HttpPost("/create-role")]
        public async Task<ActionResult> Create(string roleName)
        {
            if (ModelState.IsValid)
            {
                var item = await _roleService.CreateRole(roleName);
                return Ok(item);
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("/get-all-role")]
        public async Task<ActionResult> GetAll()
        {
            if (ModelState.IsValid)
            {
                var item = await _roleService.GetAll();
                return Ok(item);
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpPut("/update-role")]
        public async Task<ActionResult> UpdateRole(UpdateRoleRequest request)
        {
            var roleUpdate = await _roleService.UpdateRole(request);
            return Ok(roleUpdate);
        }


        [AllowAnonymous]
        [HttpDelete("/delete-role")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _roleService.DeleteRole(id);
            return Ok(user);
        }
    }
}
