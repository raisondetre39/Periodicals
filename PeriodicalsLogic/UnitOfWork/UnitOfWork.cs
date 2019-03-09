using Periodicals.DAL.DbHelpers;
using Periodicals.DAL.Repository;
using System;

namespace Periodicals.DAL.UnitOfWork
{
    // Class incapsulates all entity managers in the form of properties and stores the general data context.
    public class UnitOfWork : IDisposable
    {
        private PeriodicalsContext db;
        private HostRepository hostRepository;

        public UnitOfWork(string connectionString)
        {
            db = new PeriodicalsContext(connectionString);
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

        public void SaveAsync()
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
            if (!this.disposed)
            {
                if (disposing)
                {
                    hostRepository.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
