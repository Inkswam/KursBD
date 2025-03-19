using System.Collections.ObjectModel;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.ViewModels
{
    public class BookingViewModel
    {
        public ObservableCollection<Booking> Bookings { get; set; }

        public BookingViewModel()
        {
            Bookings = new ObservableCollection<Booking>();
        }
    }
}