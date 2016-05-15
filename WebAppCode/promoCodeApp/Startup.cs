using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(promoCodeApp.Startup))]
namespace promoCodeApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
