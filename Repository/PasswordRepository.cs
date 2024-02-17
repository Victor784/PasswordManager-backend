using Microsoft.EntityFrameworkCore;
using PassMngr.Models;
using static System.Net.Mime.MediaTypeNames;
using PassMngr.DBContext;

namespace PassMngr.Repository
{
    public class PasswordRepository : IRepository<Password>
    {
        private readonly ApplicationDbContext _context;

        public PasswordRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Password entity)
        {
            _context.Passwords.Add(entity);
            _context.SaveChangesAsync();
        }
        public Password? GetById(int id)
        {
            var password = _context.Passwords.FirstOrDefault(t => t.id == id);
            if (password != null)
            {
                return password;
            }
            return null;
        }
        public void Delete(Password entity)
        {
            _context.Passwords.Remove(entity);
            _context.SaveChanges();
        }
        public void Update(int id, Password entity)
        {
            var passToUpdate = _context.Passwords.FirstOrDefault(t => t.id == id);
            if (passToUpdate != null)
            {
                passToUpdate.associated_email = entity.associated_email != "" ? entity.associated_email : passToUpdate.associated_email;
                passToUpdate.associated_website = entity.associated_website != "" ? entity.associated_website : passToUpdate.associated_website;
                passToUpdate.password_value = entity.password_value != "" ? entity.password_value : passToUpdate.password_value;
                passToUpdate.time_of_creation = entity.time_of_creation != "" ? entity.time_of_creation : passToUpdate.time_of_creation;
                passToUpdate.time_of_last_update = entity.time_of_last_update != "" ? entity.time_of_last_update : passToUpdate.time_of_last_update;
                passToUpdate.expiration_date = entity.expiration_date != "" ? entity.expiration_date : passToUpdate.expiration_date;
                _context.SaveChanges();
            }
        }
        public List<Password> GetAll()
        {
            return _context.Passwords.ToList();
        }
    }
}
