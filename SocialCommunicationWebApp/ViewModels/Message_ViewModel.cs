using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialCommunicationWebApp.ViewModels
{
    public class Message_ViewModel
    {
        public int FromId { get; set; }
        public String FromName { get; set; }
        public int ToId { get; set; }
        public String ToName { get; set; }
        public List<String> MessageList { get; set; }
        public String MessageDetails { get; set; }
        public int Seen { get; set; }
    }
}