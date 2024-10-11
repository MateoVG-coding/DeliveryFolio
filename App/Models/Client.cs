using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.Models
{
    public partial class Client : ObservableValidator, IEditableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string phoneNumber;

        [ObservableProperty] 
        private string address;

        [ObservableProperty]
        private DateTime dateCreated = DateTime.Now;

        private Client _backupClient;

        public void BeginEdit()
        {
            _backupClient = (Client)this.MemberwiseClone();
        }

        public void CancelEdit()
        {
            if (_backupClient != null)
            {
                Id = _backupClient.Id;
                Name = _backupClient.Name;
                PhoneNumber = _backupClient.PhoneNumber;
                Address = _backupClient.Address;
                DateCreated = _backupClient.DateCreated;
            }
        }

        public void EndEdit()
        {
            _backupClient = null;
        }
    }
}
