using System.Threading.Tasks;
using PassMngr.Models;
namespace PassMngr.Helpers;


public class UsersManager
{
    public async Task<bool> MatchDBEntry(string email, string password, List<User> allUsers)
    {
       return allUsers.Any(u => u.email == email && u.password == password);
    }

    public async Task<(bool entryFound, string email)> MatchDBEntry(string token, List<User> allUsers)
    {
        foreach (var user in allUsers)
        {
            string expectedToken = TokenGenerator.GenerateToken(user.id, user.email);
            if (expectedToken == token)
            {
                return (true, user.email);
            }
        }
        return (false, "");
    }
}
