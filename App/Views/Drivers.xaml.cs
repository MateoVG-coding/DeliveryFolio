using Courier_Data_Control_App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Courier_Data_Control_App.Pages
{
    /// <summary>
    /// Interaction logic for Drivers.xaml
    /// </summary>
    public partial class Drivers : Page
    {
        public Drivers()
        {
            InitializeComponent();
            var app = (App)Application.Current;
            DataContext = app.ServiceProvider.GetRequiredService<DriversViewModel>();
        }
    }
}
