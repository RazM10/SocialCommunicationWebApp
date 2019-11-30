using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialCommunicationWebApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public String MessageDetails { get; set; }
        public int Seen { get; set; }
    }
}