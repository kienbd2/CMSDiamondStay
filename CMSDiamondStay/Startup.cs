using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CMSDiamondStay.Startup))]
namespace CMSDiamondStay
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
