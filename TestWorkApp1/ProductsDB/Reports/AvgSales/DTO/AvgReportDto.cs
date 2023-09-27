using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWorkApp1.ProductsDB.Reports.AvgSales.DTO
{
    public class AvgReportDto
    {
        public Guid ShopId { get; set; }
        public string ShopTitle { get; set; }
        public Guid ProductId { get; set; }
        public string ProductBarcode { get; set; }
        public double ProductPrice { get; set; }
        public string ProductTitle { get; set; }
        public int SumProductCount { get; set; }
        public int RealDaysCount { get; set; }
        public DateTime SaleDay { get; set; }
    }
}
