using System;
using TestWorkApp1.Common;
using TestWorkApp1.Interfaces;

namespace TestWorkApp1.ProductsDB.Models.Shop
{
    [TableName("Shops")]
    public class Shop : IGuidIdentity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
    }
}
