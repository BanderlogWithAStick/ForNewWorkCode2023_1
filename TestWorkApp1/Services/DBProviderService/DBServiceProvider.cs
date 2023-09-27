using System.Data;
using Npgsql;
using System.Configuration;
using System;

namespace TestWorkApp1.Services.DBProviderService
{
    public class ProductsDBServiceProvider
    {
        public static NpgsqlConnection GetDbConnection()
        {
            ConfigurationManager.RefreshSection("connectionStrings");
            return new NpgsqlConnection(ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString);
        }
    }
}
