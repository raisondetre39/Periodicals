using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.Repository;

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
