using System;
using TestWorkApp1.Common;
using TestWorkApp1.Interfaces;

namespace TestWorkApp1.ProductsDB.Models.Product
{
    [TableName("Products")]
    public class Product : IGuidIdentity
    {
        public Guid Id { get; set; }
        public string Barcode { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
