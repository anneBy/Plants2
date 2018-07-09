using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Plants2.Startup))]
namespace Plants2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
