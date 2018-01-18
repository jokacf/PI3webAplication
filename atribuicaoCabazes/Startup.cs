using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(atribuicaoCabazes.Startup))]
namespace atribuicaoCabazes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
