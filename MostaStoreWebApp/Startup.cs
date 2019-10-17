using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MostaStoreWebApp.Startup))]
namespace MostaStoreWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
