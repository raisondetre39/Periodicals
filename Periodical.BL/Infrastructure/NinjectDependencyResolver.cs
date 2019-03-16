using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;

namespace Periodical.BL.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IHostService>().To<HostService>();
            kernel.Bind<IMagazineService>().To<MagazineService>();
            kernel.Bind<IHostMagazineService>().To<HostMagazineService>();
            kernel.Bind<ITagService>().To<TagService>();
            kernel.Bind<IAdminService>().To<AdminService>();
        }
    }
}
