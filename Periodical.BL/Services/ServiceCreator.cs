using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Periodical.BL.Services
{
    public class ServiceCreator
    {
        public HostService CreateHostService(string connection)
        {
            return new HostService(new UnitOfWork(connection));
        }

        public MagazineService CreateMagazineService(string connection)
        {
            return new MagazineService(new UnitOfWork(connection));
        }

        public TagService CreateTagService(string connection)
        {
            return new TagService(new UnitOfWork(connection));
        }

        public HostMagazineService CreateHostMagazineService(string connection)
        {
            return new HostMagazineService(new UnitOfWork(connection));
        }
    }
}

