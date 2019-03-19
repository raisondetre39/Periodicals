using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Periodical.BL.Services;
using Ninject.Web.Common;
using Periodical.BL.ServiseInterfaces;

namespace Periodical.BL.Infrastructure
{
    /// <summary>
    /// Class creates DI container to transfet all services to presentation layer
    /// </summary>
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

        /// <summary>
        /// Method add to kernel all binds to services
        /// </summary>
        public void AddBindings()
        {
            kernel.Bind<IHostService>().To<HostService>().InRequestScope();
            kernel.Bind<IMagazineService>().To<MagazineService>().InRequestScope();
            kernel.Bind<IHostMagazineService>().To<HostMagazineService>().InRequestScope();
            kernel.Bind<ITagService>().To<TagService>().InRequestScope();
            kernel.Bind<IAdminService>().To<AdminService>().InRequestScope();
        }
    }
}
