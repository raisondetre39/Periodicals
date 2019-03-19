using Periodicals.DAL.Accounts;
using Periodicals.DAL.DbHelpers;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.Repository;
using System;

namespace Periodicals.DAL.UnitOfWork
{
    /// <summary>
    ///  Class incapsulates all entity managers in the form of properties and share the general data context to them
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private PeriodicalsContext db;

        public UnitOfWork()
        {
            db = new PeriodicalsContext("DefaultConnection");
        }

        public IGenericRepository<Tag> TagRepository { get { return new GenericRepository<Tag>(db); } }

        public IGenericRepository<Magazine> MagazineRepository { get{ return new GenericRepository<Magazine>(db); } }

        public IGenericRepository<Host> HostRepository{ get { return new GenericRepository<Host>(db); } }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    TagRepository.Dispose();
                    MagazineRepository.Dispose();
                    HostRepository.Dispose();
                    db.Dispose();
                }
                disposed = true;
            }
        }
    }
}
