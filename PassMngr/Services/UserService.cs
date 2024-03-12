using PassMngr.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PassMngr.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using PassMngr.Helpers;
using System.ComponentModel.DataAnnotations;
using Logger;

namespace PassMngr.Services
{
    [Route("api/users")]
    [ApiController]
    public class UserService : ControllerBase
    {
        private readonly UserRepository repository;
        private readonly UsersManager userManager;
        private readonly LoggerService logger;
        public UserService(IRepository<User> repo ,LoggerService logger)
        {
            repository = (UserRepository)repo;
            userManager = new UsersManager();
            this.logger = logger;
        }

        [HttpGet("")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            logger.Log("UserService : HttpGet");
            return repository.GetAll();
        }

        [HttpGet("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<User> GetById(int id)
        {
            logger.Log("UserService : HttpGet(id)");
            var user = repository.GetById(id);
            if (user == null)
            {
                logger.Log("UserService : HttpGet(id) : User not found");
                return NotFound(); 
            }
            return user;
        }

        [HttpPost]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<User> Create(User user)
        {
            logger.Log("UserService : HttpPost");

            var myTest = repository.GetAll();
            if (repository.GetAll().Any(u => u.email == user.email))
            {
                logger.Log("UserService : HttpPost : Email already exists");
                return Conflict("Email already exists");
            }

            user.id = repository.GetAll().Count() + 1;
            HashingService hashingService = new HashingService(logger);
            user.password = hashingService.HashString(hashingService.addPepper(user.password));
            repository.Add(user);
            logger.Log("UserService : HttpPost : Created new user");
            return CreatedAtAction(nameof(GetById), new { id = user.id }, user);
        }

        [HttpPut("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<User> Update(int id, User updatedUser)
        {
            logger.Log("UserService : HttpPut(id)");
            var user = repository.GetById(id);
            if (user == null)
            {
                logger.Log("UserService : HttpPut(id) : User not found");
                return NotFound();
            }
                
            repository.Update(id, updatedUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult Delete(int id)
        {
            logger.Log("UserService : HttpDelete(id)");
            var user = repository.GetById(id);
            if (user == null)
            {
                logger.Log("UserService : HttpDelete(id) : User not found");
                return NotFound();
            }
            repository.Delete(user);
            return NoContent();
        }

        [HttpPost("auth")]
        [EnableCors("PassMngrPolicy")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] LoginRequest model)
        {
            logger.Log("UserService : HttpPost/auth");
            if (!ModelState.IsValid)
            {
                logger.Log("UserService : HttpPost/auth : Invalid JSON body format");
                return BadRequest(ModelState);
            }
            bool isAuthenticated = await AuthenticateUserAsync(model.Email, model.Password);

            if (isAuthenticated)
            {
                int userId = repository.getByEmail(model.Email).id;
                if(userId != null)
                {
                    logger.Log("UserService : HttpPost/auth : User is authenticated");
                    return Ok(new { Message = "Authentication successful", UserId = userId });
                }
                else
                {
                    logger.Log("UserService : HttpPost/auth : User is authenticated but user id not found in DB");
                    return BadRequest(new { Message = "Authentication failed" });
                }
                    
            }
            else
            {
                logger.Log("UserService : HttpPost/auth : User is not authenticated - invalid email or password");
                return Unauthorized(new { Message = "Invalid email or password" });
            }
        }

        private async Task<bool> AuthenticateUserAsync(string email, string password)
        {
            logger.Log("UserService : HttpPost/auth : AuthenticateUserAsync");
            HashingService hashingService = new HashingService(logger);
            List<string> peppers = hashingService.getAllPeppers();
            bool matchesDBEntry = false;
            //TODO: delete this after testing is done
            if (email == "test@test.test" || email == "test1@test.test") 
                return true;
            foreach (string pepper in peppers)
            {
                var hashedPassword = hashingService.HashString(hashingService.addPepper(password, pepper));
                matchesDBEntry = await userManager.MatchDBEntry(email, hashedPassword, repository.GetAll());
                if (matchesDBEntry)
                {
                    logger.Log("UserService : HttpPost/auth : AuthenticateUserAsync : Authentication successfull");
                    return true;
                }
                    
            }
            logger.Log("UserService : HttpPost/auth : AuthenticateUserAsync : Authentication NOT successfull");
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
