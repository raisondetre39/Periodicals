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

        public GenericRepository(PeriodicalsContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(int? id)
        { 
            _dbSet.Remove(_dbSet.Find(id));
            _dbContext.SaveChanges();
        }


        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public TEntity GetById(int? id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public TEntity GetOne(Func<TEntity, bool> predicate)
        {
            try
            { 
            return _dbSet.AsNoTracking().Single(predicate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Update(TEntity entity)
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
