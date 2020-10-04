using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(booksmanagement.Startup))]
namespace booksmanagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
