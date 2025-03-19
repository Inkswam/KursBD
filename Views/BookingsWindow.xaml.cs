using System.Linq;
using System.Windows;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem
{
    public partial class BookingsWindow : Window
    {
        private readonly HotelContext _context;

        public BookingsWindow()
        {
            InitializeComponent();
            _context = new HotelContext();
            LoadBookings();
        }

        private void LoadBookings()
        {
            BookingsDataGrid.ItemsSource = _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .ToList();
        }

        private void AddBooking_Click(object sender, RoutedEventArgs e)
        {
            var addBookingWindow = new AddBookingWindow();
            if (addBookingWindow.ShowDialog() == true)
            {
                LoadBookings();
            }
        }

        private void EditBooking_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsDataGrid.SelectedItem is Booking selectedBooking)
            {
                var editBookingWindow = new AddBookingWindow(selectedBooking);
                if (editBookingWindow.ShowDialog() == true)
                {
                    LoadBookings();
                }
            }
            else
            {
                MessageBox.Show("Оберіть бронювання для редагування.");
            }
        }

        private void DeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsDataGrid.SelectedItem is Booking selectedBooking)
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити бронювання?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Bookings.Remove(selectedBooking);
                    _context.SaveChanges();
                    LoadBookings();
                }
            }
            else
            {
                MessageBox.Show("Оберіть бронювання для видалення.");
            }
        }
    }
}
