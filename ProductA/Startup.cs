using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProductA.Startup))]
namespace ProductA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
