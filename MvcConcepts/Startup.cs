using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcConcepts.Startup))]
namespace MvcConcepts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
