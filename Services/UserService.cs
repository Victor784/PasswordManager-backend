using PassMngr.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PassMngr.Models;
using Microsoft.AspNetCore.Cors;

namespace PassMngr.Services
{
    [Route("api/users")]
    [ApiController]
    public class UserService : ControllerBase
    {
        private readonly UserRepository repository;

        public UserService(IRepository<User> repo) 
        {
            repository = (UserRepository)repo;
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

    }
}
