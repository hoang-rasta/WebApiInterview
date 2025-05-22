using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiBasicAuthentication.DTOs;
using WebApiBasicAuthentication.Models;
using WebApiBasicAuthentication.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiBasicAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/User
        [HttpGet("GetAllUsersAsync")]
        [Authorize(Policy = "AdminOnly")] // Only Admin can view all users
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            var users = await _userService.GetUsersAsync();
            var userDtos = users.Select(u => new UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Password = u.Password
            }).ToList();
            return Ok(userDtos);
        }

        // GET: api/User/GetUserByIdAsync/1
        [HttpGet("GetUserByIdAsync/{id}")]
        //[Authorize(Policy = "AdminOrUser")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            var userDto = new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password
            };
            return Ok(userDto);
        }
        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Create([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Map DTO -> Entity
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password
            };
            user = await _userService.CreateUserAsync(user);
            // Map Entity -> DTO
            userDto.Id = user.Id;
            return CreatedAtAction(nameof(Get), new { id = user.Id }, userDto);
        }
        // PUT: api/User/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != userDto.Id)
                return BadRequest("ID in URL doesn't match ID in payload.");
            // Map DTO -> Entity
            var user = new User
            {
                Id = userDto.Id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password
            };
            var updated = await _userService.UpdateUserAsync(user);
            if (!updated)
                return NotFound();
            return NoContent();
        }
        // DELETE: api/User/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
