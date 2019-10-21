using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialCommunicationWebApp.Models;

namespace SocialCommunicationWebApp.ViewModels
{
    public class UserCountry_ViewModel
    {
        public User User { get; set; }
        public List<Country> Countries { get; set; }
    }
}