using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialCommunicationWebApp.Startup))]
namespace SocialCommunicationWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
