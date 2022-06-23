using flash_card.business.Services;
using flash_card.data.Model.Request.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace flash_card.api.Controllers
{
    [ApiController]
    [Route("/users")]
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
        [HttpPost("/create-user")]
        public async Task<ActionResult> Create(CreateUserRequest request)
        {
            var user = await _userService.CreateUser(request);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("/get-all-user")]
        public async Task<ActionResult> GetAll()
        {
            if (User.HasClaim("Role", "Admin") || User.HasClaim("Role", "Member"))
            {
                var users = await _userService.GetAll();
                return Ok(users);
            }
            return Unauthorized("Not permission.");
        }

        [AllowAnonymous]
        [HttpGet("/get-user-by-id")]
        public async Task<ActionResult> GetById(int id)
        {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPut("/update-user")]
        public async Task<ActionResult> Update(UpdateUserRequest request)
        {
            var user = await _userService.UpdateUser(request);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpDelete("/delete-user")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userService.DeleteUser(id);
            return Ok(user);
        }
    }
}
