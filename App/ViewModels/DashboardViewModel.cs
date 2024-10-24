using CommunityToolkit.Mvvm.ComponentModel;
using Courier_Data_Control_App.Repositories;
using Courier_Data_Control_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.ViewModels
{
    /// <summary>
    /// A view model for dashboard page
    /// </summary>
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly DeliveryRepository _deliveryRepository;
        private readonly ISharedDataService _sharedDataService;


    }
}
