using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NTTUserApp.Data.Entities;
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

        

    }
}
