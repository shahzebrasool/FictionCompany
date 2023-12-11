using FictionalCompany.Entities.Models;
using FictionalCompany.Entities.Repos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FicitionalCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: api/<HomeController>
        [HttpGet]
        [Route("GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var cacheKey = "GetAllUsers";
            var cacheDuration = TimeSpan.FromMinutes(10);

            var users = await _userRepository.GetAllWithCacheAsync(cacheKey, cacheDuration);
            
            return Ok(users);

        }

        // GET api/<HomeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {

            var cacheKey = $"GetUser_{id}";
            var cacheDuration = TimeSpan.FromMinutes(10);

            var user = await _userRepository.GetByIdWithCacheAsync(id, cacheKey, cacheDuration);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST api/<HomeController>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var newUser = await _userRepository.AddAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }


        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            bool isUpdated = false;
            if (id != user.Id)
            {
                return BadRequest();
            }

            isUpdated = await _userRepository.UpdateAsync(user);
            return Ok(isUpdated);
        }

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteAsync(user);
            return NoContent();
        }
    }
}
