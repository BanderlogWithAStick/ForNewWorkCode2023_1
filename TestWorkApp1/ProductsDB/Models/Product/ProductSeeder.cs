using System.Data;
using System.Threading.Tasks;
using TestWorkApp1.Interfaces;
using Dapper;
using TestWorkApp1.Services.SQLReaderService;
using TestWorkApp1.Common;
using System;

namespace TestWorkApp1.ProductsDB.Models.Product
{
    public class ProductSeeder : ICustomSeeder
    {
        private readonly IDbConnection _connection;
        private readonly SQLReaderService _sqlReaderService;
        public ProductSeeder(
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
            int count = _connection.QuerySingle<int>($"SELECT COUNT(*) FROM public.\"{((TableNameAttribute)Attribute.GetCustomAttribute(typeof(Product), typeof(TableNameAttribute))).TableName}\"");

            if (count == 0)
            {
                _connection.ExecuteAsync(_sqlReaderService.Read(SQLReaderService.PresetSQLFiles.ProductsSeeder));
            }

            return Task.CompletedTask;
        }
    }
}
