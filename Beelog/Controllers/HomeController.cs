using Beelog.Data;
using Beelog.Models;
using Beelog.UtilityClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Beelog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if(!SignedIn.isSignedIn(HttpContext))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            var posts = _context.Posts.Where(p => p.Author == SignedIn.GetCurrentUser(HttpContext, _context)).OrderByDescending(p => p.Id);
            ViewBag.Posts = posts;
            return View();
        }

        [HttpPost]
        public IActionResult AddPost(IFormCollection fc)
        {
            Post post = new Post() { Author = SignedIn.GetCurrentUser(HttpContext, _context), Text = fc["PostText"] };
            _context.Add(post);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ViewUser(int id)
        {

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            ViewBag.User = user;
            ViewBag.Posts = _context.Posts.Where(p => p.Author == user).OrderByDescending(p => p.Id);
            return View();
        }

        [HttpPost]
        [Route("Post/LikePost")]
        public JsonResult LikePost(int postId)
        {
            User user = SignedIn.GetCurrentUser(HttpContext, _context);
            int userId = user.Id;
            bool hasLiked = _context.Posts.FirstOrDefault(p => p.Id == postId).Likes.Contains(user);

            if (!hasLiked)
            {
                _context.Posts.FirstOrDefault(p => p.Id == postId).Likes.Add(user);
            }

            int likeCount = _context.Posts.FirstOrDefault(p => p.Id == postId).Likes.Count();
            return Json(new { LikeCount = likeCount });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}