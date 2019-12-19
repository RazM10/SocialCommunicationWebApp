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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            List<User> userList = _context.UsercSet.ToList();
            foreach (User _user in userList)
            {
                if (_user.Email == user.Email && _user.Password == user.Password)
                {
                    Session["email"] = user.Email;
                    Session["password"] = user.Password;
                    return RedirectToAction("Home", "User");
                }
            }
            return RedirectToAction("Login", "User");
        }

        public ActionResult Home()
        {
            if (Session["email"] != null)
            {
                String email = (string) Session["email"];

                User user = _context.UsercSet.SingleOrDefault(x => x.Email == email);
                List<User> users = _context.UsercSet.ToList();

                users.Remove(user);

                List<Friend> friends = _context.Friends.ToList();
                foreach (Friend friend in friends)
                {
                    if (friend.UserFromId == user.Id)
                    {
                        User user1 = _context.UsercSet.SingleOrDefault(x => x.Id == friend.UserToId);
                        users.Remove(user1);
                    }
                    else if (friend.UserToId == user.Id)
                    {
                        User user1 = _context.UsercSet.SingleOrDefault(x => x.Id == friend.UserFromId);
                        users.Remove(user1);
                    }
                }

                CountNotification();

                return View(users);
            }

            return RedirectToAction("Login");
        }

        public ActionResult FriendRequest()
        {
            CountNotification();
            if (Session["email"] != null)
            {
                UserFriend_ViewModel userFriendViewModel = CountFriendRequests();
                return View(userFriendViewModel);
            }

            return RedirectToAction("Login");
        }

        public ActionResult FriendsList()
        {
            CountNotification();
            if (Session["email"] != null)
            {
                String email = (string) Session["email"];

                User user = _context.UsercSet.SingleOrDefault(x => x.Email == email);

                List<Friend> friends = _context.Friends.ToList();
                List<User> users = _context.UsercSet.ToList();

                List<Friend> friendList = new List<Friend>();
                List<String> fromFriendNameList = new List<string>();

                foreach (Friend friend in friends)
                {
                    if ((friend.UserToId == user.Id || friend.UserFromId==user.Id) & friend.Accept == 1)
                    {
                        friendList.Add(friend);
                        User user1=new User();
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
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session["email"] = null;
            Session["password"] = null;
            return RedirectToAction("Login");
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
                if ((message.ToId==user.Id) & (message.Seen==0))
                {
                    count++;
                }
            }

            ViewBag.message = count;
        }
    }
}