using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using SocialCommunicationWebApp.Models;
using SocialCommunicationWebApp.ViewModels;

namespace SocialCommunicationWebApp.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _context;

        public UserController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: User
        public ActionResult Index()
        {
            var c = _context.UsercSet.Include(m => m.Country).ToList();
            return View(c);
        }

        public ActionResult New()
        {
            var countries = _context.Countries.ToList();
            var viewModel = new UserCountry_ViewModel()
            {
                User = new User(),
                Countries = countries
            };
            //return View("CustomerForm",viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult New(User user)
        {
            _context.UsercSet.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "User");
        }
    }
}