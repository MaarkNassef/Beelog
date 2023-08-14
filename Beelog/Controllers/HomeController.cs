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
            var posts = _context.Posts.Where(p => p.Author == SignedIn.GetCurrentUser(HttpContext, _context))
                .Include(p => p.Likes).OrderByDescending(p => p.Id);
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

        //ViewUser

        [HttpGet]
        public IActionResult ViewUser(int id)
        {
            if (!SignedIn.isSignedIn(HttpContext))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            ViewBag.User = user;
            ViewBag.EnableEditProfile = (id == Convert.ToInt32(HttpContext.Session.GetString("uid")));
            ViewBag.HttpContext = HttpContext;
            ViewBag.Posts = _context.Posts.Include(p => p.Likes).Where(p => p.Author == user).OrderByDescending(p => p.Id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile ProfilePicture)
        {
            if (!SignedIn.isSignedIn(HttpContext))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            var userId = Convert.ToInt32(HttpContext.Session.GetString("uid"));
            if (ProfilePicture == null || ProfilePicture.Length == 0)
            {
                return RedirectToAction("ViewUser", new { id = userId });
            }

            string oldProfilePicturePath = _context.Users.FirstOrDefault(u => u.Id == userId).ProfilePicturePath;
            if (!string.IsNullOrEmpty(oldProfilePicturePath))
            {
                string oldFilePath = Path.Combine(webHostEnvironment.WebRootPath, "images", "profile_images", oldProfilePicturePath);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            string uniqueFileName = $"{Guid.NewGuid()}_{ProfilePicture.FileName}";
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "profile_images");
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ProfilePicture.CopyToAsync(stream);
            }


            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            user.ProfilePicturePath = uniqueFileName;
            _context.Update(user);
            _context.SaveChanges();

            return RedirectToAction("ViewUser", new { id = userId });
        }
    }
}