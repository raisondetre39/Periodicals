using Periodicals.DAL.DbHelpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Periodicals.DAL.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class, new()
    {
        private readonly PeriodicalsContext _dbContext;
        private DbSet<TEntity> _dbSet;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GenericRepository(PeriodicalsContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }

        public void Create(TEntity entity)
        {
            try
            {
                log.Info($"Request to create new {typeof(TEntity)} in databse");
                _dbSet.Add(entity);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                log.Warn(ex);
            }
        }

        public void Delete(int? id)
        {
            try
            {
                log.Info("Request to delete entity from databse");
                _dbSet.Remove(_dbSet.Find(id));
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                log.Warn(ex);
            }

        }

        public IEnumerable<TEntity> GetAll()
        {
            try
            {
                log.Info($"Request to get all entities from {typeof(TEntity)} table ");
                return _dbSet.AsNoTracking().ToList();
            }
            catch(Exception ex)
            {
                log.Warn(ex);
                return null;
            }
            
        }

        public TEntity GetById(int? id)
        {
            try
            {
                log.Info($"Request to get {typeof(TEntity)} by {id} from databse");
                return _dbSet.Find(id);
            }
            catch(Exception ex)
            {
                log.Warn(ex);
                return null;
            }
            
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            try
            {
                log.Info($"Request to get {typeof(TEntity)}s by condition: {predicate} from databse");
                return _dbSet.AsNoTracking().Where(predicate).ToList();
            }
            catch(Exception ex)
            {
                log.Warn(ex);
                return null;
            }
        }

        public TEntity GetOne(Func<TEntity, bool> predicate)
        {
            try
            {
                log.Info($"Request to get {typeof(TEntity)} by condition: {predicate} from databse");
                return _dbSet.AsNoTracking().Single(predicate);
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return null;
            }
        }

        public void Update(TEntity entity)
        {
            try
            {
                log.Info($"Request to updatae {typeof(TEntity)} in databse");
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                log.Warn(ex);
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
