using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialCommunicationWebApp.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserFromId { get; set; }
        public int UserToId { get; set; }
        public int Accept { get; set; }
        public string Date { get; set; }
    }
}