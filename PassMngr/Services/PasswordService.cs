using PassMngr.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PassMngr.Models;
using Microsoft.AspNetCore.Cors;

namespace PassMngr.Services
{
    [Route("api/passwords")]
    [ApiController]
    public class PasswordService : ControllerBase
    {
        private readonly PasswordRepository repository;

        public PasswordService(IRepository<Password> repo)
        {
            repository = (PasswordRepository)repo;
        }

        [HttpGet]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<IEnumerable<Password>> getPasswords()
        {
            var allPasswords = repository.GetAll();
            EncryptionService encryptService = new EncryptionService();
            allPasswords.ForEach(pass => pass.password_value = encryptService.DecryptString(pass.password_value));
            return allPasswords;
        }

        [HttpGet("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<Password> GetById(int id)
        {
            var pass = repository.GetById(id);
            if (pass == null)
                return NotFound();
            return pass;
        }

        [HttpPost]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<Password> Create(Password pass)
        {
            pass.id = repository.GetAll().Count() + 1;
            EncryptionService encryptService = new EncryptionService();
            pass.password_value = encryptService.EncryptString(pass.password_value);
            repository.Add(pass);

            return CreatedAtAction(nameof(GetById), new { id = pass.id }, pass);
        }

        [HttpPut("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<Password> Update(int id, Password updatedPass)
        {
            var pass = repository.GetById(id);
            if (pass == null)
                return NotFound();

            pass = updatedPass;
            repository.Update(id, updatedPass);
            return Ok(updatedPass);
        }

        [HttpDelete("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult Delete(int id)
        {
            var pass = repository.GetById(id);
            if (pass == null)
                return NotFound();

            repository.Delete(pass);
            return Ok();
        }

    }
}
