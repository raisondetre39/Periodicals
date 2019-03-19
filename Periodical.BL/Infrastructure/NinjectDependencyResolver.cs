using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Periodical.BL.Services;
using Ninject.Web.Common;
using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.Repository;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.UnitOfWork;
using Periodicals.DAL;
using Periodicals.DAL.DbHelpers;

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
            kernel.Bind<IGenericRepository<Magazine>>().To<GenericRepository<Magazine>>();
            kernel.Bind<IGenericRepository<Tag>>().To<GenericRepository<Tag>>();
            kernel.Bind<IGenericRepository<Host>>().To<GenericRepository<Host>>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<IDbContext>().To<PeriodicalsContext>().InRequestScope();
            kernel.Bind<IHostService>().To<HostService>().InRequestScope();
            kernel.Bind<IMagazineService>().To<MagazineService>().InRequestScope();
            kernel.Bind<IHostMagazineService>().To<HostMagazineService>().InRequestScope();
            kernel.Bind<ITagService>().To<TagService>().InRequestScope();
            kernel.Bind<IAdminService>().To<AdminService>().InRequestScope();
        }
    }
}
