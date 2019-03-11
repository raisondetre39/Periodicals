using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Periodical.BL.Services;

[assembly: OwinStartup(typeof(Periodicals.Startup))]

namespace Periodicals
{
    public class Startup
    {
        ServiceCreator serviceCreator = new ServiceCreator();
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(CreateHostService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

        public HostService CreateHostService()
        {
            return serviceCreator.CreateHostService("DefaultConnection");
        }

        public MagazineService CreateMagazineService()
        {
            return serviceCreator.CreateMagazineService("DefaultConnection");
        }

        public TagService CreateTagService()
        {
            return serviceCreator.CreateTagService("DefaultConnection");
        }

        public HostMagazineService CreateHostMagazineService()
        {
            return serviceCreator.CreateHostMagazineService("DefaultConnection");
        }
    }
}