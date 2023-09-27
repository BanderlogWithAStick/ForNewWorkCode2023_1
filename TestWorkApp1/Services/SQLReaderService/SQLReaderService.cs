using System;
using System.IO;
using System.Text;

namespace TestWorkApp1.Services.SQLReaderService
{
    public class SQLReaderService
    {
        public SQLReaderService() { }
        public string Read(string path)
        {
            return new StreamReader($"SQL/{path}", System.Text.Encoding.UTF8).ReadToEnd();
        }
        public string Read(PresetSQLFiles preset)
        {
            switch(preset)
            {
                case PresetSQLFiles.Startup: return new StreamReader($"SQL/Startup.sql", Encoding.UTF8).ReadToEnd();
                case PresetSQLFiles.ProductsSeeder: return new StreamReader($"SQL/SeedersData/Products.sql", Encoding.UTF8).ReadToEnd();
                case PresetSQLFiles.ShopsSeeder: return new StreamReader($"SQL/SeedersData/Shops.sql", Encoding.UTF8).ReadToEnd();

                default: throw new Exception($"In {nameof(SQLReaderService)} use unknown {nameof(PresetSQLFiles)} type.");
            }
        }

        public enum PresetSQLFiles
        {
            Startup = 0,
            ProductsSeeder = 1,
            ShopsSeeder = 2,
        }
    }
}
