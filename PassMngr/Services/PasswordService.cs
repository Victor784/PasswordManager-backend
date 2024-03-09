using PassMngr.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PassMngr.Models;
using Microsoft.AspNetCore.Cors;
using Logger;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PassMngr.Services
{
    [Route("api/passwords")]
    [ApiController]
    public class PasswordService : ControllerBase
    {
        private readonly PasswordRepository repository;
        private readonly LoggerService logger;

        public PasswordService(IRepository<Password> repo , LoggerService logger)
        {
            repository = (PasswordRepository)repo;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<IEnumerable<Password>> getPasswords(int id)
        {
            logger.Log("PasswordService : HttpGet(id)");
            var allPasswords = repository.GetAll().Where(pass => pass.user_id == id).ToList();
            //TODO uncomment this after testing is done
            //EncryptionService encryptService = new EncryptionService();
            //allPasswords.ForEach(pass => pass.password_value = encryptService.DecryptString(pass.password_value));
            return allPasswords;
        }

        public ActionResult<Password> GetById(int id)
        {
            logger.Log("PasswordService : GetById(id)");
            var pass = repository.GetById(id);
            if (pass == null)
            {
                logger.Log("PasswordService : GetById(id) : password not found");
                return NotFound();
            }
            return pass;
        }

        [HttpPost]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<Password> Create(Password pass)
        {
            logger.Log("PasswordService : HttpPost");
            pass.id = repository.GetAll().Count() + 1;
            EncryptionService encryptService = new EncryptionService(logger);
            pass.password_value = encryptService.EncryptString(pass.password_value);
            repository.Add(pass);

            return CreatedAtAction(nameof(GetById), new { id = pass.id }, pass);
        }

        [HttpPut("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult<Password> Update(int id, Password updatedPass)
        {
            logger.Log("PasswordService : HttpPut(id)");
            var pass = repository.GetById(id);
            if (pass == null)
            {
                logger.Log("PasswordService : HttpPut(id) : Password not found");
                return NotFound();
            }
       
            pass = updatedPass;
            repository.Update(id, updatedPass);
            return Ok(updatedPass);
        }

        [HttpDelete("{id}")]
        [EnableCors("PassMngrPolicy")]
        public ActionResult Delete(int id)
        {
            logger.Log("PasswordService : HttpDelete(id)");
            var pass = repository.GetById(id);
            if (pass == null)
            {
                logger.Log("PasswordService : HttpDelete(id) : Password not found");
                return NotFound();
            }
            repository.Delete(pass);
            return Ok();
        }

    }
}
