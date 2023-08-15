using Beelog.Data;
using Beelog.Models;
using Beelog.UtilityClasses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Beelog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public HomeController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            if(!SignedIn.isSignedIn(HttpContext))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            //var posts = _context.Posts.Where(p => p.Author == SignedIn.GetCurrentUser(HttpContext, _context))
            //    .Include(p => p.Likes).OrderByDescending(p => p.Id);

            var currentUserId = Convert.ToInt32(HttpContext.Session.GetString("uid"));
            var currentUser = _context.Users.Include(u => u.Following).FirstOrDefault(u => u.Id == currentUserId);
            var posts = _context.Posts.Include(p => p.Likes)
                .Where(p => currentUser.Following.Contains(p.Author)).OrderByDescending(p => p.Id);

            ViewBag.Posts = posts;
            ViewBag.HttpContext = HttpContext;
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

        [HttpPost]
        public IActionResult AddComments(IFormCollection fc)
        {
            var user = SignedIn.GetCurrentUser(HttpContext, _context);
            var PostId = Convert.ToInt32(fc["PostId"]);
            Comment comment = new Comment()
            {
                PostId = PostId,
                Post = _context.Posts.FirstOrDefault(p => p.Id == PostId),
                UserId = user.Id,
                User = user,
                Text = fc["CommentText"]
            };
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("Post/GetComments")]
        public IActionResult GetComments(int postId)
        {
            List<Comment> comments = _context.Comments.Include(c => c.User).Where(c => c.PostId == postId)
                .OrderByDescending(c => c.Id).ToList();
            foreach (var comment in comments)
            {
                comment.User.Comments = null;
            }
            return Json(comments);
        }

        [HttpPost]
        [Route("Post/LikePost")]
        public JsonResult LikePost(int postId)
        {
            User user = SignedIn.GetCurrentUser(HttpContext, _context);
            int userId = user.Id;
            bool hasLiked = _context.Likes.Where(l => l.PostId == postId && l.UserId == userId).ToList().Count != 0;

            if (!hasLiked)
            {
                _context.Likes.Add(new Like() { 
                    Post = _context.Posts.FirstOrDefault(p => p.Id == postId),
                    PostId = postId,
                    UserId = userId,
                    User = user
                });
                _context.SaveChanges();
            }
            else
            {
                _context.Likes.Remove(
                    _context.Likes.FirstOrDefault(l => l.UserId == userId && l.PostId == postId)
                );
                _context.SaveChanges();
            }

            int likeCount = _context.Posts.Include(p => p.Likes).FirstOrDefault(p => p.Id == postId).Likes.Count();
            return Json(new { LikeCount = likeCount });
        }
        [HttpPost]
        [Route("Post/GetLikes")]
        public JsonResult GetLikes(int postId)
        {
            var post = _context.Posts.Include(p => p.Likes).FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                int likeCount = post.Likes.Count();
                return Json(new { LikeCount = likeCount });
            }
            return Json(new { LikeCount = 0 });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}