using Dapper;
using DapperRepository.Interfaces;
using System;
using System.Collections;
using System.Linq;

namespace DapperRepository
{
    public abstract partial class DataBase : IDataBase
    {

        public virtual void Insert<TEntity>(TEntity entity)
        {
            DBInsert(entity);
        }

        private void DBInsert(object entity)
        {
            ConnectionOpen();

            BeginTransaction();

            string query = BuildInsertQuery(entity);

            int PrimaryKey = Connection.ExecuteScalar<int>(query + " SELECT LAST_INSERT_ID();", entity, transaction: Transaction);
            if (PrimaryKey > 0)
            {
                entity.GetType().GetProperty("Id").SetValue(entity, PrimaryKey);

                var childProperties = entity.GetType().GetProperties().Where(t => t.PropertyType.IsGenericType);
                foreach (var propriedade in childProperties)
                {
                    var childValues = ((IEnumerable)propriedade.GetValue(entity, null)).Cast<object>();
                    foreach (var child in childValues)
                    {
                        var ForeignKey = child.GetType().GetProperty(entity.GetType().Name + "Id");
                        if (ForeignKey != null) ForeignKey.SetValue(child, PrimaryKey);
                        DBInsert(child);
                    }
                }
            }
            else
            {
                throw new Exception("Insert error.");
            };
        }

        private string BuildInsertQuery(object entity)
        {
            var cache = new QueryCache(QueryCacheType.Insert, Connection, entity.GetType());

            if (!DataBaseQueryCache.TryGetValue(cache, out var query))
            {
                var columns = GetColumns(entity);
                var stringOfColumns = string.Join(", ", columns);
                var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
                query = $" INSERT INTO {entity.GetType().Name} ({stringOfColumns}) VALUES ({stringOfParameters});";
                DataBaseQueryCache.TryAdd(cache, query);
            }
            return query;
        }
    }
}
