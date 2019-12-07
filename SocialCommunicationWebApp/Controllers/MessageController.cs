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
            Message_ViewModel messageViewModel=new Message_ViewModel();
            if (Session["email"] != null)
            {
                String email = (string)Session["email"];

                User user = _context.UsercSet.SingleOrDefault(x => x.Email == email);
                if (user.Id == id)
                {
                    messageViewModel.FromId = id;
                    messageViewModel.FromName = user.Name;
                    User user1 = _context.UsercSet.SingleOrDefault(x => x.Id == id2);
                    messageViewModel.ToId = id2;
                    messageViewModel.ToName = user1.Name;
                }
                else
                {
                    messageViewModel.FromId = id2;
                    messageViewModel.FromName = user.Name;
                    User user1 = _context.UsercSet.SingleOrDefault(x => x.Id == id);
                    messageViewModel.ToId = id;
                    messageViewModel.ToName = user1.Name;
                }

                List<String> messageList = new List<String>();
                List<Message> messages = _context.Messages.ToList();
                foreach (Message message in messages)
                {
                    if ((message.FromId == messageViewModel.FromId & message.ToId == messageViewModel.ToId) | (message.ToId == messageViewModel.FromId & message.FromId == messageViewModel.ToId))
                    {
                        messageList.Add(message.MessageDetails);
                    }
                }

                messageViewModel.MessageList = messageList;
                return View(messageViewModel);
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public ActionResult SendingMessage(Message message)
        {
            message.Seen = 0;
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("Index", "Message");
        }
    }
}