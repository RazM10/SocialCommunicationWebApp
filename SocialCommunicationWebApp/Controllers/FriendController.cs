using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialCommunicationWebApp.Models;

namespace SocialCommunicationWebApp.Controllers
{
    public class FriendController : Controller
    {
        private ApplicationDbContext _context;

        public FriendController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Friend
        public ActionResult Index()
        {
            List<User> users =_context.UsercSet.ToList();
            return View(users);
        }

        public ActionResult FriendRequest(int id)
        {
            Friend friend=new Friend();
            string email = (string) Session["email"];
            User user = _context.UsercSet.SingleOrDefault(u => u.Email == email);

            if (user != null)
            {
                friend.UserFromId = user.Id;
                friend.UserToId = id;
            }

            return RedirectToAction("Index");
        }
    }
}