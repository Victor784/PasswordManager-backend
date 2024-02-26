using Microsoft.EntityFrameworkCore;
using PassMngr.Models;
using static System.Net.Mime.MediaTypeNames;
using PassMngr.DBContext;
using PassMngr.Services;

namespace PassMngr.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChangesAsync();
        }
        public User? GetById(int id) 
        {
            var user = _context.Users.FirstOrDefault(t => t.id == id);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public User? getByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(t => t.email == email);
            if (user != null)
            {
                return user;
            }
            return null;
        }
        public void Delete(User entity) 
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();    
        }
        public void Update(int id, User entity) 
        {
            var userToUpdate = _context.Users.FirstOrDefault(t => t.id == id);
            if (userToUpdate != null)
            {
                userToUpdate.email = entity.email != "" ? entity.email : userToUpdate.email;
                userToUpdate.password = entity.password != "" ? entity.password : userToUpdate.password;
                userToUpdate.name = entity.name != "" ? entity.name : userToUpdate.name;
                userToUpdate.surname = entity.surname != "" ? entity.surname : userToUpdate.surname;
                userToUpdate.date_of_registration = entity.date_of_registration != "" ? entity.date_of_registration : userToUpdate.date_of_registration;
                userToUpdate.is_active = entity.is_active != null ? entity.is_active : userToUpdate.is_active;
                userToUpdate.is_confirmed = entity.is_confirmed != null ? entity.is_confirmed : userToUpdate.is_confirmed;
                _context.SaveChanges();
            }
        }
        public List<User> GetAll() 
        {
            return _context.Users.ToList();
        }

    }
}
