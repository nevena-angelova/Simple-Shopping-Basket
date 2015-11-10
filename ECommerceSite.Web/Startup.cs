using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ECommerceSite.Startup))]
namespace ECommerceSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
