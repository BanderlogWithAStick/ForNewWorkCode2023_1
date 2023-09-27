using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TestWorkApp1.Common;
using TestWorkApp1.Interfaces;
using TestWorkApp1.ProductsDB.Models.Product;
using TestWorkApp1.ProductsDB.Models.Shop;
using TestWorkApp1.Services.SQLReaderService;

namespace TestWorkApp1.ProductsDB.Models.Sale
{
    public class SaleSeeder : ICustomSeeder
    {
        private readonly IDbConnection _connection;
        public SaleSeeder(
            IDbConnection connection,
            SQLReaderService sqlReaderService
            )
        {
            _connection = connection;
        }
        public int SeederPriority => 200;

        public async Task Seed()
        {
            int count = _connection.QuerySingle<int>($"SELECT COUNT(*) FROM public.\"{((TableNameAttribute)Attribute.GetCustomAttribute(typeof(Sale), typeof(TableNameAttribute))).TableName}\"");

            if (count == 0)
            {
                Random rnd = new Random();
                List<Shop.Shop> shops = _connection.Query<Shop.Shop>("SELECT * FROM public.\"Shops\"").AsList();
                List<Product.Product> products = _connection.Query<Product.Product>("SELECT * FROM public.\"Products\"").AsList();
                List<Sale> salesForCreate = new List<Sale>();
                // Сгенерируем для каждого магазина по несколько проданных товаров за последние 28 дней
                foreach (Shop.Shop shop in shops)
                {
                    for (DateTime i = DateTime.Now; i > DateTime.Now.AddDays(-28); i = i.AddDays(-1))
                    {
                        int productInThisDay = rnd.Next(0, 30);
                        for (int p = 0; p < productInThisDay; p++)
                        {
                            salesForCreate.Add(new Sale
                            {
                                ShopId = shop.Id,
                                SaleMoment = i.AddHours(rnd.Next(-23, 0)).AddMinutes(rnd.Next(-59, 0)).AddSeconds(rnd.Next(-59, 0)),
                                ProductCount = rnd.Next(1, 10),
                                ProductId = products[rnd.Next(0, products.Count - 1)].Id,
                            });
                        }
                    }
                }

                // Отправим в базу созданные объекты
                StringBuilder sb = new StringBuilder().Append("INSERT INTO public.\"Sales\"(\"ShopId\", \"ProductId\", \"ProductCount\", \"SaleMoment\") VALUES ");
                for (int i = 0; i < salesForCreate.Count; i++)
                {
                    sb.Append($"\n('{salesForCreate[i].ShopId}', '{salesForCreate[i].ProductId}', {salesForCreate[i].ProductCount}, '{salesForCreate[i].SaleMoment}'){(i == salesForCreate.Count - 1 ? ';' : ',')} ");
                }
                string sql = sb.ToString();
                await _connection.ExecuteAsync(sql);
            }

            return;
        }
    }
}
