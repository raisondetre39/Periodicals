﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Periodicals.DAL.Repository
{
    /// <summary>
    ///  Class gives acsses for methods to work with dada base
    /// </summary>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class, new()
    {
        private IDbContext _dbContext;
        private DbSet<TEntity> _dbSet;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GenericRepository(IDbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }
        

        /// <summary>
        ///  Method creates new instanse in dat base
        /// </summary>
        public void Create(TEntity entity)
        {
             _dbContext.Set<TEntity>().Add(entity);
             _dbContext.SaveChanges();
        }

        /// <summary>
        ///  Method removes instance from data base
        /// </summary>
        public void Delete(int? id)
        {
            _dbSet.Remove(_dbSet.Find(id));
            _dbContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        /// <summary>
        ///  Method retuns all TEntity instanses from data base
        /// </summary>
        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        /// <summary>
        ///  Method retuns TEntity instanse by its id from data base
        /// </summary>
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

        /// <summary>
        ///  Method retuns all TEntity instanses annaproperiate to paticular condition from data base
        /// </summary>
        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            try
            {
                log.Info($"Request to get {typeof(TEntity)}s by condition: {predicate} from databse");
                return _dbSet.Where(predicate).ToList();
            }
            catch(Exception ex)
            {
                log.Warn(ex);
                return null;
            }
        }

        /// <summary>
        ///  Method retuns TEntity instanse match to paticular condition from data base
        /// </summary>
        public TEntity GetOne(Func<TEntity, bool> predicate)
        {
            try
            {
                log.Info($"Request to get {typeof(TEntity)} by condition: {predicate} from databse");
                return _dbSet.Single(predicate);
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return null;
            }
        }

        /// <summary>
        ///  Method updates TEntity instanse in data base
        /// </summary>
        public void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
