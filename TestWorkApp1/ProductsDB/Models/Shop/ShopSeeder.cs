using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;
using TestWorkApp1.Common;
using TestWorkApp1.Interfaces;
using TestWorkApp1.Services.SQLReaderService;

namespace TestWorkApp1.ProductsDB.Models.Shop
{
    public class ShopSeeder : ICustomSeeder
    {
        private readonly IDbConnection _connection;
        private readonly SQLReaderService _sqlReaderService;
        public ShopSeeder(
            IDbConnection connection,
            SQLReaderService sqlReaderService
            )
        {
            _connection = connection;
            _sqlReaderService = sqlReaderService;
        }
        public int SeederPriority => 100;

        public Task Seed()
        {
            int count = _connection.QuerySingle<int>($"SELECT COUNT(*) FROM public.\"{((TableNameAttribute)Attribute.GetCustomAttribute(typeof(Shop), typeof(TableNameAttribute))).TableName}\"");

            if (count == 0)
            {
                _connection.ExecuteAsync(_sqlReaderService.Read(SQLReaderService.PresetSQLFiles.ShopsSeeder));
            }

            return Task.CompletedTask;
        }
    }
}
