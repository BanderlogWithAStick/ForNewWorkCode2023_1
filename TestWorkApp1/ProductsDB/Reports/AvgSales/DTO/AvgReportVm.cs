using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWorkApp1.ProductsDB.Reports.AvgSales.DTO
{
    public class AvgReportVm
    {
        /// <summary>
        /// Guid магазина
        /// </summary>
        public Guid ShopId { get; set; }
        /// <summary>
        /// Название магазина
        /// </summary>
        public string ShopTitle { get; set; }
        /// <summary>
        /// Guid продукта
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// Штрихкод продукта
        /// </summary>
        public string ProductBarcode { get; set; }
        /// <summary>
        /// Наименование продукта
        /// </summary>
        public string ProductTitle { get; set; }
        /// <summary>
        /// Общее количество проданных продуктов
        /// </summary>
        public int SumProductCount { get; set; }
        /// <summary>
        /// Среднее количество продуктов в день
        /// </summary>
        public double SumAvgCount { get; set; }
        /// <summary>
        /// Общая сумма продаж
        /// </summary>
        public double SumPrice { get; set; }
        /// <summary>
        /// Средняя сумма продаж за крайние 7 дней (не дальше 14ти дней)
        /// </summary>
        public double SumAvgPrice { get; set; }
    }
}
