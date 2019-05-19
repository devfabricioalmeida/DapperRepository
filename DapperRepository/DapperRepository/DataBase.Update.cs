using DapperRepository.Interfaces;
using System;

namespace DapperRepository
{
    public abstract partial class DataBase : IDataBase
    {
        public void Update<TEntity>(TEntity entity)
        {

            var PrimaryKey = (int)entity.GetType().GetProperty("Id").GetValue(entity);

            if (PrimaryKey <= 0)
                throw new Exception("Update not allowed without a defined Id.");

            ConnectionOpen();

            BeginTransaction();
        }
    }
}
