using Beelog.Data;
using Beelog.Models;
using Beelog.UtilityClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beelog.Controllers
{
    public class AuthenticationController : Controller
    {
        private ApplicationDbContext _context;

        public AuthenticationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(IFormCollection fc)
        {
            string hashPwd = HashUtility.HashPassword(fc["Password"]);
            if (_context.Users.FirstOrDefault(u => u.Email.Equals(fc["Email"])) != null)
            {
                ViewBag.emailMsg = "Email is already used.";
                ViewBag.Name = fc["Name"];
                ViewBag.Email = fc["Email"];
                return View();
            }
            User user = new User() { Name = fc["Name"], Email = fc["Email"], Password = hashPwd };
            _context.Add(user);
            _context.SaveChanges();
            HttpContext.Session.SetString("uid", _context.Users.FirstOrDefault(u => u.Email.Equals(user.Email)).Id.ToString());
            return RedirectToAction("Index", "Home");
        }        
        
        [HttpGet]
        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        [Route("SignIn")]
        public IActionResult SignIn(IFormCollection fc)
        {
            string hashPwd = HashUtility.HashPassword(fc["Password"]);
            User user = _context.Users.FirstOrDefault(u => u.Email.Equals(fc["Email"])&&u.Password.Equals(hashPwd));
            if (user == null)
            {
                ViewBag.emailMsg = "Wrong email or password.";
                ViewBag.Email = fc["Email"];
                return View();
            }
            HttpContext.Session.SetString("uid", _context.Users.FirstOrDefault(u => u.Email.Equals(user.Email)).Id.ToString());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Authentication");
        }
    }
}
