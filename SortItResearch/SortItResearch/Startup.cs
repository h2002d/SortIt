using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SortItResearch.Startup))]
namespace SortItResearch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
