using DapperRepository.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Reflection;

namespace DapperRepository
{
    public abstract partial class DataBase : IDataBase
    {
        internal struct QueryCache : IEquatable<QueryCache>
        {
            public QueryCache(QueryCacheType cacheType, IDbConnection connection, MemberInfo memberInfo)
            {
                ConnectionType = connection.GetType();
                CacheType = cacheType;
                MemberInfo = memberInfo;
            }

            public QueryCacheType CacheType { get; }

            public Type ConnectionType { get; }

            public MemberInfo MemberInfo { get; }

            public bool Equals(QueryCache other) => CacheType == other.CacheType && ConnectionType == other.ConnectionType && MemberInfo == other.MemberInfo;
        }

        internal enum QueryCacheType
        {
            Get,
            GetById,
            GetAll,
            Insert,
            Update,
            Delete,
        }
        internal static ConcurrentDictionary<QueryCache, string> DataBaseQueryCache { get; } = new ConcurrentDictionary<QueryCache, string>();
    }
}
