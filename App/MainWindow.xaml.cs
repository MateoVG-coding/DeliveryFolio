using System;
using System.Collections.Generic;
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

namespace Courier_Data_Control_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl && tabControl.SelectedItem is TabItem selectedTab)
            {
                string tag = selectedTab.Tag as string;

                switch (tag)
                {
                    case "Home":
                        //fContainer.Navigate(new Uri("Pages/HomePage.xaml", UriKind.Relative));
                        break;
                    case "Deliveries":
                        fContainer.Navigate(new Uri("Deliveries.xaml", UriKind.Relative));
                        break;
                    case "Clients":
                        //fContainer.Navigate(new Uri("Pages/ClientsPage.xaml", UriKind.Relative));
                        break;
                    case "Couriers":
                        fContainer.Navigate(new Uri("Drivers.xaml", UriKind.Relative));
                        break;
                    case "Settings":
                        //fContainer.Navigate(new Uri("Pages/SettingsPage.xaml", UriKind.Relative));
                        break;
                }
            }
        }

        // Start: Button Close | Restore | Minimize 
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // End: Button Close | Restore | Minimize
    }
}
