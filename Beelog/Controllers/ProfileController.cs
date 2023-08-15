﻿using Beelog.Data;
using Beelog.Models;
using Beelog.UtilityClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beelog.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProfileController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }


        public IActionResult Index()
        {
            if (!SignedIn.isSignedIn(HttpContext))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            var userId = Convert.ToInt32(HttpContext.Session.GetString("uid"));
            return RedirectToAction("ViewUser", new { id = userId });
        }


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
            return View("Index");
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

        [HttpPost]
        public IActionResult EditBio(IFormCollection fc)
        {
            var userId = Convert.ToInt32(HttpContext.Session.GetString("uid"));
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            user.Bio = fc["BioText"];
            _context.Update(user);
            _context.SaveChanges();
            return RedirectToAction("ViewUser", new { id = userId });
        }
    }
}
