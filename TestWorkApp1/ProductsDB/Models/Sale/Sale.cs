using System;
using TestWorkApp1.Common;
using TestWorkApp1.Interfaces;

namespace TestWorkApp1.ProductsDB.Models.Sale
{
    [TableName("Sales")]
    public class Sale : IGuidIdentity
    {
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public Guid ProductId { get; set; }
        public long ProductCount { get; set; }
        public DateTime SaleMoment { get; set; }
    }
}
