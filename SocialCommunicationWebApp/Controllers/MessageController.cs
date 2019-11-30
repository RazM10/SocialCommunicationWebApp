using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialCommunicationWebApp.Models;
using SocialCommunicationWebApp.ViewModels;

namespace SocialCommunicationWebApp.Controllers
{
    public class MessageController : Controller
    {
        private ApplicationDbContext _context;

        public MessageController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Message
        public ActionResult Index()
        {
            if (Session["email"] != null)
            {
                String email = (string)Session["email"];

                User user = _context.UsercSet.SingleOrDefault(x => x.Email == email);

                List<Friend> friends = _context.Friends.ToList();
                List<User> users = _context.UsercSet.ToList();

                List<Friend> friendList = new List<Friend>();
                List<String> fromFriendNameList = new List<string>();

                foreach (Friend friend in friends)
                {
                    if ((friend.UserToId == user.Id || friend.UserFromId == user.Id) & friend.Accept == 1)
                    {
                        friendList.Add(friend);
                        User user1 = new User();
                        if (friend.UserToId == user.Id)
                        {
                            int id = friend.UserFromId;
                            user1 = _context.UsercSet.SingleOrDefault(x => x.Id == id);
                        }
                        else
                        {
                            int id = friend.UserToId;
                            user1 = _context.UsercSet.SingleOrDefault(x => x.Id == id);
                        }


                        fromFriendNameList.Add(user1.Name);
                    }
                }

                UserFriend_ViewModel userFriendViewModel = new UserFriend_ViewModel()
                {
                    User = user,
                    Friends = friendList,
                    FromFriendNameList = fromFriendNameList
                };

                return View(userFriendViewModel);
            }
            return RedirectToAction("Login","User");
        }

        public ActionResult MessageFromTo(int id, int id2)
        {
            //String email = (string)Session["email"];
            //User user = _context.UsercSet.SingleOrDefault(x => x.Email == email);
            //if (user.Id == id) { }
            return View();
        }
    }
}