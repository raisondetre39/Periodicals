﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Periodical.BL.Services;

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
            kernel.Bind<HostService>().ToSelf();
            kernel.Bind<MagazineService>().ToSelf();
            kernel.Bind<HostMagazineService>().ToSelf();
            kernel.Bind<TagService>().ToSelf();
            kernel.Bind<AdminService>().ToSelf();
        }
    }
}