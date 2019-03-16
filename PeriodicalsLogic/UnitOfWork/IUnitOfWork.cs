using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Periodicals.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Tag> TagRepository { get; }

        IGenericRepository<Magazine> MagazineRepository { get; }

        IGenericRepository<Host> HostRepository { get; }

        void Save();
    }
}
