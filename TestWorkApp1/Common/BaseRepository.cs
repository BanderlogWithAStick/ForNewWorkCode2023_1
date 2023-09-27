using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using TestWorkApp1.Interfaces;
using TestWorkApp1.ProductsDB.Models.Sale;
using TestWorkApp1.ProductsDB.Models.Shop;
using TestWorkApp1.Services.SQLReaderService;
using static Dapper.SqlMapper;

namespace TestWorkApp1.Common
{
    public class BaseRepository<T> : IRepository<T> where T : IGuidIdentity
    {
        private readonly IDbConnection _connection;
        public BaseRepository(
            IDbConnection connection
            )
        {
            _connection = connection;
        }
        public List<T> GetList()
        {
            List<T> records = _connection.Query<T>($"SELECT * FROM public.\"{((TableNameAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableNameAttribute))).TableName}\"").AsList();
            return records;
        }
    }
}
