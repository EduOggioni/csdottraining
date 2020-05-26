using csdottraining.Models;
using csdottraining.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace csdottraining.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get() =>
           await _userService.GetUsersAsync();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _userService.GetUsersAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<User>> Create(User user)
        {
            await _userService.CreateAsync(user);
            
            return CreatedAtRoute("GetUser", new { id = user.id.ToString() }, user);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User userIn)
        {
            var user = await _userService.GetUsersAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.UpdateAsync(id, userIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetUsersAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.RemoveAsync(user.id);

            return NoContent();
        }
    }
}