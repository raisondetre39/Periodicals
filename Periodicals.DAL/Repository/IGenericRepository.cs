using System;
using System.Collections.Generic;

namespace Periodicals.DAL.Repository
{
    public interface IGenericRepository<TEntity>
        where TEntity : class, new()
    {
        void Create(TEntity entity);

        void Delete(int? id);

        void Delete(TEntity entity);

        TEntity GetById(int? id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

        TEntity GetOne(Func<TEntity, bool> predicate);

        void Update(TEntity entity);

        void Dispose();
    }
}
