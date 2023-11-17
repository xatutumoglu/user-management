using Microsoft.AspNetCore.Mvc;
using NTTUserApp.Service.Abstractions;
using NTTUserApp.Service.Models.Users;

namespace NTTUserApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpGet]
        public async Task<ActionResult<List<UserModel>>> GetAllUsers()
        {
            var user = await _userService.GetUsersAsync();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser(CreateUserRequest user)
        {
            var item = await _userService.CreateUserAsync(user);
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<UserModel>> UpdateUser(UpdateUserRequest user)
        {
            var item = await _userService.UpdateUserAsync(user);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteUser([FromRoute]DeleteUserRequest user)
        {
            var item = await _userService.DeleteUserAsync(user);
            if (!item)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}