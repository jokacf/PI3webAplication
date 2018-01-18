using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AtribuicaoCabazesipps.Startup))]
namespace AtribuicaoCabazesipps
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
