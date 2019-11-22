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
            if (Session["email"] != null)
            {
                List<User> users = _context.UsercSet.ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        public ActionResult FriendRequest(int id)
        {
            Friend friend = new Friend();
            string email = (string)Session["email"];
            User user = _context.UsercSet.SingleOrDefault(u => u.Email == email);

            if (user != null)
            {
                friend.UserFromId = user.Id;
                friend.UserToId = id;
                friend.Accept = 0;
                friend.Date = DateTime.Now.ToString("yyyy-MM-dd");

                _context.Friends.Add(friend);
                _context.SaveChanges();
                return RedirectToAction("Home", "User");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        public ActionResult AcceptFriendRequest(int id)
        {
            Friend friendInDb = _context.Friends.SingleOrDefault(f => f.Id == id);
            if (friendInDb != null) friendInDb.Accept = 1;
            _context.SaveChanges();
            return RedirectToAction("Home", "User");
        }
    }
}