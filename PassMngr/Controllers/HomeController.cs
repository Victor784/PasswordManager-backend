using Microsoft.AspNetCore.Mvc;
using PassMngr.Models;
using System.Diagnostics;
using PassMngr.DBContext;
using Microsoft.EntityFrameworkCore;

namespace PassMngr.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //ADD
            //User test = new User(1, "test", "test", "test", "test", 1, false, false);
            //_context.Users.Add(test);
            //_context.SaveChangesAsync();

            //READ
            //var user = _context.Users.Include(t => t.password_list).ToList();

            //UPDATE
            //var userToUpdate = _context.Users.FirstOrDefault(t => t.name == "test");
            //if (userToUpdate != null)
            //{
            //    userToUpdate.name = "Updated test";
            //    _context.SaveChanges();
            //}

            //DELETE
            //var userToDelete = _context.Users.FirstOrDefault(t => t.name == "test");
            //if (userToDelete != null)
            //{
            //    _context.Users.Remove(userToDelete);
            //    _context.SaveChanges();
            //}

            //ADD
            //Password passwordTest = new Password(1, 1, "test", "test", 1, 1, 1);
            //_context.Passwords.Add(passwordTest);
            //_context.SaveChangesAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser()
        {

            //Password passwordTest = new Password(1, 1, "test", "test", 1, 1, 1);
            //List<Password> passwordListTest = null;
            //passwordListTest.Add(passwordTest);
            User test = new User(1, "test", "test", "test", "test", "", false, false);
            _context.Users.Add(test);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}