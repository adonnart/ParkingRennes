using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjetParking.Startup))]
namespace ProjetParking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
