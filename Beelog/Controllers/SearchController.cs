using Beelog.Data;
using Beelog.Models;
using Beelog.UtilityClasses;
using Microsoft.AspNetCore.Mvc;

namespace Beelog.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public JsonResult GetSimilarNames(string searchTerm)
        {
            if (searchTerm == null)
            {
                return Json(new { });
            }
            searchTerm = searchTerm.Trim();
            if (searchTerm.Equals(""))
            {
                return Json(new { });
            }
            var similarNames = _context.Users
                .Where(u => u.Name.ToLower().Contains(searchTerm.ToLower()))
                .Select(u => new { Id = u.Id, Name = u.Name , ProfilePicturePath = u.ProfilePicturePath })
                .ToList();

            return Json(similarNames);
        }


        public IActionResult Index()
        {
            if (!SignedIn.isSignedIn(HttpContext))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            return View();
        }
    }
}
