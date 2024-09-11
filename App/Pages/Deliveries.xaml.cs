using Courier_Data_Control_App.Classes;
using Courier_Data_Control_App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

namespace Courier_Data_Control_App.Pages
{
    /// <summary>
    /// Interaction logic for Deliveries.xaml
    /// </summary>
    public partial class Deliveries : Page
    {
        public Deliveries()
        {
            InitializeComponent();

            var app = (App)Application.Current;
            DataContext = app.ServiceProvider.GetRequiredService<DeliveriesViewModel>();
        }
    }
}
