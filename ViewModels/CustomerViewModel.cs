using System.Collections.ObjectModel;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.ViewModels
{
    public class CustomerViewModel
    {
        public ObservableCollection<Customer> Customers { get; set; }

        public CustomerViewModel()
        {
            Customers = new ObservableCollection<Customer>();
        }
    }
}