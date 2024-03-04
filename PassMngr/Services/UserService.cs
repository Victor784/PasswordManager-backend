using PassMngr.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PassMngr.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using PassMngr.Helpers;
using System.ComponentModel.DataAnnotations;

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

        [HttpPost]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<User> Create(User user)
        {
            user.id = repository.GetAll().Count() + 1;
            HashingService hashingService = new HashingService();
            user.password = hashingService.HashString(hashingService.addPepper(user.password));
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
            //TODO Delete the model.Email != "test" && model.Password != "test"
            if (!ModelState.IsValid && model.Email != "test" && model.Password != "test")
            {
                return BadRequest(ModelState);
            }
            bool isAuthenticated = await AuthenticateUserAsync(model.Email, model.Password);

            if (isAuthenticated)
            {
           
                int userId = repository.getByEmail(model.Email).id;
                if(userId != null)
                    return Ok(new { Message = "Authentication successful", UserId = userId });
                else
                    return BadRequest(new { Message = "Authentication failed" });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }
        }

        private async Task<bool> AuthenticateUserAsync(string email, string password)
        {
            HashingService hashingService = new HashingService();
            List<string> peppers = hashingService.getAllPeppers();
            bool matchesDBEntry = false;
            //TODO: delete this after testing is done
            if (email == "test" || email == "test1") 
                return true;
            foreach (string pepper in peppers)
            {
                var hashedPassword = hashingService.HashString(hashingService.addPepper(password, pepper));
                matchesDBEntry = await userManager.MatchDBEntry(email, hashedPassword, repository.GetAll());
                if (matchesDBEntry)
                    return true;
            }          
            return false;
        }

        }
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
