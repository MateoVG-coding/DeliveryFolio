using Courier_Data_Control_App.Models;
using Courier_Data_Control_App.ViewModels;
using MaterialDesignThemes.Wpf;
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

namespace Courier_Data_Control_App
{
    /// <summary>
    /// Interaction logic for Deliveries.xaml
    /// </summary>
    public partial class Drivers : Page
    {
        public Drivers()
        {
            InitializeComponent();
        }

        private void DialogHostEditDriver_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            var viewModel = this.DataContext as DriversViewModel;

            if (eventArgs.Parameter is bool result && result)
            {
                if (viewModel?.UpdateDriverCommand.CanExecute(null) == true)
                {
                    viewModel.UpdateDriverCommand.Execute(null);
                }
            }
            else
            {
                // Call cancel edit
                viewModel.CancelEdit();
            }
        }

        private void DialogHostAddDriver_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            var viewModel = this.DataContext as DriversViewModel;

            if (eventArgs.Parameter is bool result && result)
            {
                if (viewModel?.AddDriverCommand.CanExecute(null) == true)
                {
                    viewModel.AddDriverCommand.Execute(null);
                }
            }
            else
            {
                viewModel.NewDriver = new Driver();
            }
        }

        private void Button_Click_FlipOtherDriversFlipper(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;

            var listBoxItem = FindAncestor<System.Windows.Controls.ListBoxItem>(button);

            listBoxItem.IsSelected = true;

            FlipBackAllFlippers(AvailableDriverListBox);
            FlipBackAllFlippers(UnavailableDriverListBox);
        }
        private void FlipBackAllFlippers(System.Windows.Controls.ListBox listBox)
        {
            var listBoxItems = listBox.Items.Cast<Driver>().ToList();
            foreach (var item in listBoxItems)
            {
                var itemContainer = listBox.ItemContainerGenerator.ContainerFromItem(item) as System.Windows.Controls.ListBoxItem;
                if (itemContainer != null)
                {
                    var otherFlipper = FindChild<Flipper>(itemContainer);
                    if (otherFlipper != null)
                    {
                        otherFlipper.IsFlipped = false;
                    }
                }
            }
        }

        private void Button_Click_SelectDriverImage(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as DriversViewModel;

            var button = sender as System.Windows.Controls.Button;

            var listBoxItem = FindAncestor<System.Windows.Controls.ListBoxItem>(button);

            listBoxItem.IsSelected = true;

            viewModel.LoadDriverImageCommand.Execute(null);
        }

        private void DriverImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var image = sender as Image;
            var viewModel = this.DataContext as DriversViewModel;

            viewModel.CurrentDriver.ImagePath = null;

            if (image != null)
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DefaultProfilePicture.png"));
            }
            
        }

        // Helper method to find the ancestor of a specific type
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        // Helper method to find a child of a specific type within a parent
        private static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }

                var childOfChild = FindChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }
    }
}
