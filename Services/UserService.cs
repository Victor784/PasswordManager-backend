using PassMngr.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PassMngr.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using PassMngr.Helpers;

namespace PassMngr.Services
{
    [Route("api/users")]
    [ApiController]
    public class UserService : ControllerBase
    {
        private readonly UserRepository repository;
        private readonly UsersManager userManager;
        public UserService(IRepository<User> repo) 
        {
            repository = (UserRepository)repo;
            userManager = new UsersManager();
        }

        [HttpGet("")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return repository.GetAll();
        }

        [HttpGet("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<User> GetById(int id)
        {
            var user = repository.GetById(id);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<User> Create(User user)
        {
            user.id = repository.GetAll().Count() + 1;
            repository.Add(user);

            return CreatedAtAction(nameof(GetById), new { id = user.id }, user);
        }

        [HttpPut("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<User> Update(int id, User updatedUser)
        {
            var user = repository.GetById(id);
            if (user == null)
                return NotFound();
            repository.Update(id, updatedUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult Delete(int id)
        {
            var user = repository.GetById(id);
            if (user == null)
                return NotFound();

            repository.Delete(user);
            return NoContent();
        }

        [HttpPost("auth")]
        [EnableCors("PassMngrPolicy")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool isAuthenticated = await AuthenticateUserAsync(model.Email, model.Password);

            if (isAuthenticated)
            {
                int userId = 123;
                return Ok(new { Message = "Authentication successful", UserId = userId });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }
        }

        private async Task<bool> AuthenticateUserAsync(string email, string password)
        {
            bool matchesDBEntry = await userManager.MatchDBEntry(email, password, repository.GetAll());
            return matchesDBEntry ? true : false;
        }

        }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
