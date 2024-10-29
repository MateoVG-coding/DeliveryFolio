using CommunityToolkit.Mvvm.ComponentModel;
using Courier_Data_Control_App.Models;
using Courier_Data_Control_App.Repositories;
using Courier_Data_Control_App.Services;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Courier_Data_Control_App.ViewModels
{
    /// <summary>
    /// A view model for dashboard page
    /// </summary>
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly DeliveryRepository _deliveryRepository;
        private readonly ISharedDataService _sharedDataService;

        [ObservableProperty]
        private SeriesCollection seriesOverWeekDeliveriesCollection;

        [ObservableProperty]
        private SeriesCollection seriesTodayDeliveriesCollection;

        [ObservableProperty]
        private SeriesCollection topClientsSeries;

        [ObservableProperty]
        private List<string> topClientsNames;

        [ObservableProperty]
        private SeriesCollection topDriversSeries;

        [ObservableProperty]
        private List<string> topDriversNames;

        [ObservableProperty]
        private List<int> deliveriesCount;

        [ObservableProperty]
        private List<string> daysLabels;

        [ObservableProperty]
        private int todayDeliveries;

        public DashboardViewModel(DeliveryRepository deliveryRepository, ISharedDataService sharedDataService)
        {
            _deliveryRepository = deliveryRepository;
            _sharedDataService = sharedDataService;

            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            await LoadCountDeliveriesDataAsync();
            await LoadDeliveriesOverWeekChartDataAsync();
            await LoadTopClientsChartDataAsync();
            await LoadTopDriversChartDataAsync();
        }

        private async Task LoadDeliveriesOverWeekChartDataAsync()
        {
            var deliveries = await _deliveryRepository.GetDeliveriesForLast7DaysAsync();

            var deliveriesGroupedByDate = deliveries
                .GroupBy(d => d.DateCreated.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToDictionary(g => g.Date, g => g.Count);

            var lastSevenDays = Enumerable.Range(0, 7)
                                          .Select(offset => DateTime.Today.AddDays(-offset))
                                          .OrderBy(date => date)
                                          .ToList();

            var seriesValues = lastSevenDays.Select(date =>
                deliveriesGroupedByDate.TryGetValue(date, out var count) ? count : 0).ToList();

            SeriesOverWeekDeliveriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Entregas: ",
                    Values = new ChartValues<int>(seriesValues),
                }
            };

            DaysLabels = lastSevenDays.Select(d => d.ToString("ddd", new CultureInfo("es-ES"))).ToList();
        }

        private async Task LoadTopClientsChartDataAsync()
        {
            var deliveries = await _deliveryRepository.GetDeliveriesForLast7DaysAsync();

            var topClients = deliveries
                .GroupBy(d => string.IsNullOrWhiteSpace(d.CustomerName) ? d.PhoneNumber : d.CustomerName)
                .Select(g => new
                {
                    ClientName = g.Key,
                    DeliveryCount = g.Count(),
                    PhoneNumber = g.FirstOrDefault()?.PhoneNumber
                })
                .OrderBy(c => c.DeliveryCount)
                .Take(5)
                .ToList();

            TopClientsSeries = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "Entregas: ",
                    Values = new ChartValues<ObservableValue>(topClients.Select(c => new ObservableValue(c.DeliveryCount))),
                }
            };

            TopClientsNames = topClients.Select(c => string.IsNullOrWhiteSpace(c.ClientName) ? c.PhoneNumber : c.ClientName).ToList();
        }

        private async Task LoadTopDriversChartDataAsync()
        {
            var deliveries = await _deliveryRepository.GetDeliveriesForLast7DaysAsync();

            var topDrivers = deliveries
                .GroupBy(d => d.Driver.FullName)
                .Select(g => new
                {
                    DriverName = g.Key,
                    DeliveryCount = g.Count()
                })
                .OrderBy(d => d.DeliveryCount)
                .Take(5)
                .ToList();

            TopDriversSeries = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "Entregas: ",
                    Values = new ChartValues<ObservableValue>(topDrivers.Select(d => new ObservableValue(d.DeliveryCount))),
                }
            };

            TopDriversNames = topDrivers.Select(d => d.DriverName).ToList();
        }

        private async Task LoadCountDeliveriesDataAsync()
        {
            var totalCount = await _deliveryRepository.GetFilteredDeliveriesCountAsync(null, DateTime.Today);
            var pendingCount = await _deliveryRepository.GetPendingDeliveriesCountAsync(DateTime.Today);
            var deliveredCount = totalCount - pendingCount;

            SeriesTodayDeliveriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Entregado",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(deliveredCount) },
                },
                new PieSeries
                {
                    Title = "Pendiente",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(pendingCount) },
                    Fill = new SolidColorBrush(Colors.DarkRed)
                }
            };
        }
    }
}
