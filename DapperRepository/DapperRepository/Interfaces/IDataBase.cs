using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DapperRepository.Interfaces
{
   public interface IDataBase : IDisposable
    {
        void Insert<TEntity>(TEntity entity);
        void Update<TEntity>(TEntity entity);
        IEnumerable<TEntity> Select<TEntity>(Expression<Func<TEntity, bool>> predicate);
        void Save();
    }
}
