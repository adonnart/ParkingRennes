using Microsoft.Owin;
using Owin;
using ProjetParking.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(ProjetParking.Startup))]
namespace ProjetParking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            Timer timer = null;
            Task.Factory.StartNew(()=>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                timer = new Timer((e) =>
                {
                    new HomeController().GetStats();
                }, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            });
        }
    }
}
