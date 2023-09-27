using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWorkApp1.ProductsDB.Reports.AvgSales.DTO;

namespace TestWorkApp1.Interfaces
{
    public interface IReportService
    {
        Task<List<AvgReportVm>> GetAvgReport(List<Guid> shops, List<Guid> products, DateTime start, DateTime end);
    }
}
