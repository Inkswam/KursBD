using System.Collections.ObjectModel;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Room> Rooms { get; set; }

        public MainViewModel()
        {
          
        }
    }
}