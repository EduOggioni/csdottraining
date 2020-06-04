using csdottraining.Models;
using csdottraining.Services;
using csdottraining.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace csdottraining.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{id:length(24)}", Name = "GetUser")]
        [Authorize]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            StringValues authToken;

            var user = await _userService.GetUserByIdAsync(id);

            if(user == null) return NotFound();

            HttpContext.Request.Headers.TryGetValue("Authorization", out authToken);

            var bearerToken = authToken.ToString().Split()[1];

            if(!bearerToken.Equals(user.access_token)){
                HttpContext.Response.Headers.Add(
                    "WWW-Authenticate",
                    "error_description=\"Unauthorized user\""
                );

                return Unauthorized();
            }
            
            user.password = null;

            return user;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult<SigninResponse>> SignIn([FromBody] SigninRequest body)
        {
            var user = await _userService.GetUserAsync(body.email, body.password);
            
            if (user == null) return BadRequest(new { message = "Username or password is incorrect" });

            await _userService.UpdateAsync(user);

            return Ok(new {
                user.id,
                user.created_at,
                user.updated_at,
                user.last_login,
                user.access_token,
            });
        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<SignupResponse>> SignUp([FromBody] SignupResquest body)
        {
            var userAtBase = await _userService.GetUserAsync(body.email);
            
            if(!(userAtBase == null)) return BadRequest(new { message = "Email already exists" });

            var user = new User{
                name = body.name,
                email = body.email,
                password = body.password,
                phones = body.phones,
            };

            var createdUser = await _userService.CreateAsync(user);
            
            return CreatedAtRoute(
                "GetUser",
                new { id = createdUser.id.ToString() }, 
                new {
                    createdUser.id,
                    createdUser.created_at,
                    createdUser.updated_at,
                    createdUser.last_login,
                    createdUser.access_token,
                }
            );
        }
    }
}