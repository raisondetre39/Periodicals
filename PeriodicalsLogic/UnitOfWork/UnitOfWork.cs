using Periodicals.DAL.Accounts;
using Periodicals.DAL.DbHelpers;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.Repository;
using System;

namespace Periodicals.DAL.UnitOfWork
{
    // Class incapsulates all entity managers in the form of properties and stores the general data context.
    public class UnitOfWork : IDisposable
    {
        private PeriodicalsContext db;

        public UnitOfWork(string connectionString)
        {
            db = new PeriodicalsContext(connectionString);
        }

        public GenericRepository<Tag> TagRepository { get { return new GenericRepository<Tag>(db); } set { } }

        public GenericRepository<Magazine> MagazineRepository { get { return new GenericRepository<Magazine>(db); } set { } }

        public GenericRepository<Host> HostRepository { get { return new GenericRepository<Host>(db); } set { } }

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
                }
                disposed = true;
            }
        }
    }
}
