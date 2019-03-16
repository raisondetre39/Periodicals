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
        private readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GenericRepository(PeriodicalsContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }

        public GenericRepository() { }

        public virtual void Create(TEntity entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Delete(int? id)
        {
            _dbSet.Remove(_dbSet.Find(id));
            _dbContext.SaveChanges();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual TEntity GetById(int? id)
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

        public virtual IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
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

        public virtual TEntity GetOne(Func<TEntity, bool> predicate)
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

        public virtual void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
