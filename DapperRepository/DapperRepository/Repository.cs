using DapperRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DapperRepository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity<TEntity>
    {
        public IDataBase DataBase { get; private set; }

        public Repository(IDataBase dataBase)
        {
            DataBase = dataBase;
        }


        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return DataBase.Select<TEntity>(predicate);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DataBase.Select<TEntity>(null);
        }

        public TEntity GetbyId(int id)
        {
            return DataBase.Select<TEntity>(c => c.Id == id).FirstOrDefault();
        }

        public void SaveChanges()
        {
            DataBase.Save();
        }

        public void Update(TEntity obj)
        {
            DataBase.Update(obj);
        }

        public void Add(TEntity obj)
        {
            DataBase.Insert(obj);
        }
    }
}
