using flash_card.business.Services;
using flash_card.data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flash_card.Controllers
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
    }
}
