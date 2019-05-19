using Periodicals.DAL.Accounts;
using Periodicals.DAL.DbHelpers;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.Repository;

namespace Periodicals.DAL.UnitOfWork
{
    /// <summary>
    ///  Class incapsulates all entity managers in the form of properties and share the general data context to them
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private PeriodicalsContext db;

        public UnitOfWork( IGenericRepository<Tag> tagRepository, IGenericRepository<Magazine> magaxineRepozitory,
            IGenericRepository<Host> hostRepository)
        {
            db = new PeriodicalsContext("DefaultConnection");
            TagRepository = tagRepository;
            HostRepository = hostRepository;
            MagazineRepository = magaxineRepozitory;
        }

        public IGenericRepository<Tag> TagRepository { get; }

        public IGenericRepository<Magazine> MagazineRepository { get; }

        public IGenericRepository<Host> HostRepository{ get; }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
