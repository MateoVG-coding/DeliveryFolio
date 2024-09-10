using Courier_Data_Control_App.Repositories;
using Courier_Data_Control_App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Courier_Data_Control_App.Services;

namespace Courier_Data_Control_App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=DeliveryFolio.db"));

            services.AddScoped<DeliveryRepository>();
            services.AddScoped<DriverRepository>();
            services.AddScoped<ClientRepository>();

            services.AddSingleton<ISharedDataService>(provider =>
            {
                var deliveryRepository = provider.GetRequiredService<DeliveryRepository>();
                var driverRepository = provider.GetRequiredService<DriverRepository>();
                var clientRepository = provider.GetRequiredService<ClientRepository>();
                return new SharedDataService(deliveryRepository, driverRepository, clientRepository);
            });

            services.AddTransient<DeliveriesViewModel>();
            services.AddTransient<DriversViewModel>();
            services.AddTransient<ClientsViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
