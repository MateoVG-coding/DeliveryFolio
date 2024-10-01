﻿using Courier_Data_Control_App.Models;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;

            var listBoxItem = FindAncestor<System.Windows.Controls.ListBoxItem>(button);

            listBoxItem.IsSelected = true;

            // Find all Flipper controls in the ListBox and flip them back to front
            var listBoxItems = DriverListBox.Items.Cast<Driver>().ToList();
            foreach (var item in listBoxItems)
            {
                var itemContainer = DriverListBox.ItemContainerGenerator.ContainerFromItem(item) as System.Windows.Controls.ListBoxItem;
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
        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
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
        private T FindChild<T>(DependencyObject parent) where T : DependencyObject
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