using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProductB.Startup))]
namespace ProductB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
