using csdottraining.Models;
using csdottraining.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace csdottraining.Controllers
{
    [Route("api/v1")]
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
        [Route("users/{id:length(24)}", Name = "GetUser")]
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
        public async Task<IActionResult> SignIn([FromServices] User body)
        {
            var user = await _userService.GetUserAtBase(body.email, body.password);
            
            if (user == null) return BadRequest(new { message = "Username or password is incorrect" });

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
            var user = await _userService.GetUserByEmailAsync(body.email);
            
            if(!(user == null)) return BadRequest(new { message = "Email already exists" });

            var token = _tokenService.GenerateToken(body);

            var createdUser = await _userService.CreateAsync(body, token);

            return CreatedAtRoute(
                "GetUser",
                new { id = createdUser.id.ToString()}, 
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