using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialCommunicationWebApp.Models;
using SocialCommunicationWebApp.ViewModels;

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
            CountNotification();
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

        public UserFriend_ViewModel CountFriendRequests()
        {
            String email = (string)Session["email"];

            User user = _context.UsercSet.SingleOrDefault(x => x.Email == email);

            List<Friend> friends = _context.Friends.ToList();
            List<User> users = _context.UsercSet.ToList();

            List<Friend> friendList = new List<Friend>();
            List<String> fromFriendNameList = new List<string>();

            foreach (Friend friend in friends)
            {
                if (friend.UserToId == user.Id & friend.Accept == 0)
                {
                    friendList.Add(friend);

                    int id = friend.UserFromId;
                    User user1 = _context.UsercSet.SingleOrDefault(x => x.Id == id);

                    fromFriendNameList.Add(user1.Name);
                }
            }

            UserFriend_ViewModel userFriendViewModel = new UserFriend_ViewModel()
            {
                User = user,
                Friends = friendList,
                FromFriendNameList = fromFriendNameList
            };

            return userFriendViewModel;
        }
        public UserFriend_ViewModel CountFriendInList()
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
            return userFriendViewModel;
        }

        public void CountNotification()
        {
            UserFriend_ViewModel userFriendViewModel = CountFriendRequests();
            ViewBag.FrendRequestNumbers = userFriendViewModel.Friends.Count;
            UserFriend_ViewModel userFriendViewModel2 = CountFriendInList();
            ViewBag.NumberOfFriends = userFriendViewModel2.Friends.Count;

            List<Message> messages = _context.Messages.ToList();
            String email = (string)Session["email"];
            User user = _context.UsercSet.SingleOrDefault(x => x.Email == email);
            int count = 0;
            foreach (Message message in messages)
            {
                if ((message.ToId == user.Id) & (message.Seen == 0))
                {
                    count++;
                }
            }

            ViewBag.message = count;
        }
    }
}