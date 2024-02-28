using System.Threading.Tasks;
using PassMngr.Models;
namespace PassMngr.Helpers;


public class UsersManager
{
    public async Task<bool> MatchDBEntry(string email, string password, List<User> allUsers)
    {
       return allUsers.Any(u => u.email == email && u.password == password);
    }
}
