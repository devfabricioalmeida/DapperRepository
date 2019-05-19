using Dapper;
using DapperRepository.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DapperRepository
{
    public abstract partial class DataBase : IDataBase
    {


        public IEnumerable<TEntity> Select<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            string query = "";
            if (predicate != null)
            {
                query = "WHERE " + ConvertExpressionToString(predicate);
            }

            var entities = Connection.Query<TEntity>($"SELECT * FROM {typeof(TEntity).Name} {query} ");
            foreach (var entity in entities)
            {
                DbSelectChild(entity);
            }
            return entities;
        }

        private void DbSelectChild(object entity)
        {
            Type entityType = entity.GetType();

            int PrimaryKey = 0;
            var PropPrimaryKey = entityType.GetProperty("Id");
            string ForeignKeyName = entityType.Name;

            if (PropPrimaryKey != null)
            {
                PrimaryKey = (int)PropPrimaryKey.GetValue(entity);
                if (PrimaryKey <= 0)
                {
                    return;
                }
            }

            var childProperties = entityType.GetProperties().Where(c => c.PropertyType.IsGenericType);
            foreach (var childProperty in childProperties)
            {
             
                var childType = childProperty.PropertyType.GetGenericArguments()[0];
                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(childType);
                childProperty.SetValue(entity, Activator.CreateInstance(constructedListType));

                var childEntities = (ICollection)Connection.Query(childType, $"SELECT * FROM {childType.Name} WHERE {ForeignKeyName}Id = {PrimaryKey}; ");
                var addMethod = childProperty.PropertyType.GetMethod("Add");
                var childList = childProperty.GetValue(entity);

                foreach (var child in childEntities)
                {
                    DbSelectChild(child);
                    addMethod.Invoke(childList, new object[] { child });
                }

            }

        }

    }
}
