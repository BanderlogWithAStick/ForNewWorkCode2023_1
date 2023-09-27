using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TestWorkApp1.Interfaces;
using TestWorkApp1.ProductsDB.Models.CommonDto;
using TestWorkApp1.ProductsDB.Models.Product;
using TestWorkApp1.ProductsDB.Models.Shop;
using TestWorkApp1.ProductsDB.Reports.AvgSales.DTO;

namespace TestWorkApp1
{
    public partial class MainWindow : Window
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<Shop> _shopsRepository;
        private IReportService _reportService;
        public MainWindow(
            IRepository<Product> productsRepository,
            IRepository<Shop> shopsRepository,
            IReportService reportService
            )
        {
            _productsRepository = productsRepository;
            _shopsRepository = shopsRepository;
            _reportService = reportService;

            InitializeComponent();
        }

        public List<SimpleGuidSelectDto> Shops = new List<SimpleGuidSelectDto>();
        public List<SimpleGuidSelectDto> Products = new List<SimpleGuidSelectDto>();
        public List<AvgReportVm> ReportData = new List<AvgReportVm>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Shops = _shopsRepository.GetList().Select(x => new SimpleGuidSelectDto { Id = x.Id, Title = x.Title, IsActive = false }).ToList();
            Products = _productsRepository.GetList().Select(x => new SimpleGuidSelectDto { Id = x.Id, Title = x.Title, IsActive = false}).ToList();
            ShopsGrid.ItemsSource = Shops;
            ProductsGrid.ItemsSource = Products;
        }

        private async void GoReportBtn_Click(object sender, RoutedEventArgs e)
        {
            ReportData = await _reportService.GetAvgReport(
                Shops.Where(x => x.IsActive ?? false).Select(x => x.Id).ToList(),
                Products.Where(x => x.IsActive ?? false).Select(x => x.Id).ToList(),
                StartPeriodDP.SelectedDate ?? DateTime.Now,
                EndPeriodDP.SelectedDate ?? DateTime.Now
                );

            ReportGrid.ItemsSource = null;
            ReportGrid.ItemsSource = ReportData;
            ReportGrid.UpdateLayout();
        }
    }
}
