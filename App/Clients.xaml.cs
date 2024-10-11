using Courier_Data_Control_App.Models;
using Courier_Data_Control_App.ViewModels;
using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class Clients : Page
    {
        public Clients()
        {
            InitializeComponent();
        }

        private void DialogHostEditClient_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            var viewModel = this.DataContext as ClientsViewModel;

            if (eventArgs.Parameter is bool result && result)
            {
                if (viewModel?.UpdateClientCommand.CanExecute(null) == true)
                {
                    viewModel.UpdateClientCommand.Execute(null);
                }
            }
            else
            {
                // Call cancel edit
                viewModel.CancelEdit();
            }
        }

        private void DialogHostAddClient_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            var viewModel = this.DataContext as ClientsViewModel;

            if (eventArgs.Parameter is bool result && result)
            {
                if (viewModel?.AddClientCommand.CanExecute(null) == true)
                {
                    viewModel.AddClientCommand.Execute(null);
                }
            }
            else
            {
                viewModel.NewClient = new Client();
            }
        }

        private void Button_Click_FlipOtherClientsFlipper(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;

            var listBoxItem = FindAncestor<System.Windows.Controls.ListBoxItem>(button);

            listBoxItem.IsSelected = true;

            // Find all Flipper controls in the ListBox and flip them back to front
            var listBoxItems = ClientListBox.Items.Cast<Client>().ToList();
            foreach (var item in listBoxItems)
            {
                var itemContainer = ClientListBox.ItemContainerGenerator.ContainerFromItem(item) as System.Windows.Controls.ListBoxItem;
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
