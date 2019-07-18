using Microsoft.Owin;
using Owin;
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

            Task.Factory.StartNew(()=>
            {
                Thread.Sleep(TimeSpan.FromMinutes(10));

            });
        }
    }
}
