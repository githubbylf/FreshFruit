using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FreshFruit.WebSites.Official.Startup))]
namespace FreshFruit.WebSites.Official
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
