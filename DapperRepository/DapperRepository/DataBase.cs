using DapperRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DapperRepository
{
    public abstract partial class DataBase : IDataBase
    {
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }

        public DataBase(IDbConnection connection)
        {
            Connection = connection;
        }


        private void ConnectionOpen()
        {
            if (Connection == null || Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
        }


        private void CloseConnection()
        {
            if (Connection == null || Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }

        private void BeginTransaction()
        {
            if (Transaction == null)
            {
                Transaction = Connection.BeginTransaction();
            }
        }


        public virtual void Save()
        {
            try
            {
                Transaction.Commit();
            }
            catch (Exception)
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }


        private IEnumerable<string> GetColumns(object t)
        {
            return t.GetType().GetProperties()
                    .Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Select(e => e.Name);
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }



    }
}
