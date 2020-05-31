using csdottraining.Models;
using csdottraining.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace csdottraining.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("{id:length(24)}", Name = "GetUser")]
        [Authorize]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if(user == null) return NotFound();

            return user;
        }

        [HttpPost]
        [Route("signin")]
        [Authorize]
        public async Task<IActionResult> SignIn([FromBody] User body)
        {
            var userAtbase = await _userService.GetUserAsync(body.email, body.password);
            
            if (userAtbase == null) return BadRequest(new { message = "Username or password is incorrect" });

            var dateTime = DateTime.UtcNow;

            userAtbase.last_login = dateTime;
            userAtbase.update_date = dateTime;
            userAtbase.access_token = _tokenService.GenerateToken(body);

            var user = await _userService.UpdateAsync(userAtbase);

            return Ok(new {
                user.id,
                user.creation_date,
                user.update_date,
                user.last_login,
                user.access_token,
            });
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] User body)
        {
            var user = await _userService.GetUserAsync(body.email);
            
            if(!(user == null)) return BadRequest(new { message = "Email already exists" });
            var dateTime = DateTime.UtcNow;
                        
            body.access_token = _tokenService.GenerateToken(body);
            body.creation_date = dateTime;
            body.update_date = dateTime;

            var createdUser = await _userService.CreateAsync(body);

            return CreatedAtRoute(
                "GetUser",
                new { id = createdUser.id.ToString() }, 
                new {
                    createdUser.id,
                    createdUser.creation_date,
                    createdUser.update_date,
                    createdUser.last_login,
                    createdUser.access_token,
                }
            );
        }
    }
}