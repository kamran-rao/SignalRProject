using Microsoft.AspNetCore.Mvc;
using SignalR.Data;
using SignalR.Models;
using System.Diagnostics;

namespace SignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User newUser)
        {
            
                // Add the new user to the list
                _db.Users.Add(newUser);
                _db.SaveChanges();
                // Redirect to the UserList action to refresh the user list
                return RedirectToAction("User");
        
        }
        public IActionResult User()
        {
            // Assuming users is a List<User> containing your data
            List<User> users = _db.Users.ToList(); // fetch or create your list of users

            return View(users);
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
