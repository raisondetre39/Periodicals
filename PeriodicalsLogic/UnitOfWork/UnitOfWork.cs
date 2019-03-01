using Periodicals.DAL.DbHelpers;
using Periodicals.DAL.Repository;
using System;

namespace Periodicals.DAL.UnitOfWork
{
    // Class incapsulates all entity managers in the form of properties and stores the general data context.
    public class UnitOfWork : IDisposable
    {
        private PeriodicalContext db;
        private HostRepository hostRepository;

        public UnitOfWork(string connectionString)
        {
            db = new PeriodicalContext(connectionString);
        }

        public HostRepository Hosts
        {
            get
            {
                if (hostRepository == null)
                {
                    hostRepository = new HostRepository(db);
                }  
                return hostRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
