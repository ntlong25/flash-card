using flash_card.business.Services;
using flash_card.data.Entities;
using flash_card.data.Model.Request;
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
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<ActionResult> Login(string email, string password)
        {
            var token = await _userService.Login(email, password);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("create-user")]
        public async Task<ActionResult> Create(CreateUserRequest request)
        {
            var user = await _userService.CreateUser(request);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("/get-all-user")]
        public async Task<ActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
            
        }
    }
}
