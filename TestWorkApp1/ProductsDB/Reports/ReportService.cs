using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TestWorkApp1.Interfaces;
using TestWorkApp1.ProductsDB.Reports.AvgSales.DTO;

namespace TestWorkApp1.ProductsDB.Reports
{
    public class ReportService : IReportService
    {
        private readonly IDbConnection _connection;
        public ReportService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<AvgReportVm>> GetAvgReport(List<Guid> shops, List<Guid> products, DateTime start, DateTime end)
        {
            List<AvgReportDto> resp = (await _connection.QueryAsync<AvgReportDto>(
                    $"SELECT * FROM public.rep_avgsales ({ToDateString(start.Date)}, {ToDateString(start.Date)}, @arr_shops, @arr_products);",
                    new { arr_shops = shops, arr_products = products }
                    )
                    ).ToList();

            List<AvgReportVm> result = new List<AvgReportVm>();

            foreach (var pair in resp.GroupBy(x => $"{x.ShopId} {x.ProductId}"))
            {
                AvgReportVm vm = new AvgReportVm
                {
                    ShopId = pair.First().ShopId,
                    ShopTitle = pair.First().ShopTitle,
                    ProductId = pair.First().ProductId,
                    ProductBarcode = pair.First().ProductBarcode,
                    ProductTitle = pair.First().ProductTitle,
                    SumProductCount = pair.Sum(x => x.SumProductCount),
                    SumAvgCount = pair.Sum(x => x.SumProductCount) / pair.First().RealDaysCount,
                };

                vm.SumPrice = vm.SumProductCount * pair.First().ProductPrice;

                // Вычисляем Ср.сутку
                List<AvgReportDto> temp = pair.Where(x => start.AddDays(-14) < x.SaleDay).OrderByDescending(x => x.SaleDay).Take(7).ToList();
                vm.SumAvgPrice = (temp.Sum(x => x.SumProductCount) * pair.First().ProductPrice) / temp.Count();

                result.Add(vm);
            }

            return result;
        }

        public string ToDateString(DateTime date ) { return $"'{date.Day}.{date.Month}.{date.Year}'"; }
    }
}
