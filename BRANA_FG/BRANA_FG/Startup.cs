using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BRANA_FG.Startup))]
namespace BRANA_FG
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
