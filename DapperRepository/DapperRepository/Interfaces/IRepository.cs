using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DapperRepository.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity<TEntity>
    {
        void Add(TEntity obj);
        TEntity GetbyId(int id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity obj);
        void Delete(int id);
        IEnumerable<TEntity> Get (Expression<Func<TEntity, bool>> predicate);
        void SaveChanges();
    }
}
