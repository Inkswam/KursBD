using System;
using System.Linq;
using System.Windows;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem
{
    public partial class AddBookingWindow : Window
    {
        private readonly HotelContext _context;
        private Booking _booking;

        public AddBookingWindow(Booking booking = null)
        {
            InitializeComponent();
            _context = new HotelContext();

            GuestComboBox.ItemsSource = _context.Guests.ToList();
            GuestComboBox.DisplayMemberPath = "FullName";
            GuestComboBox.SelectedValuePath = "GuestId";

            RoomCategoryComboBox.ItemsSource = _context.Rooms
                .Select(r => r.Category)
                .Distinct()
                .ToList();

            if (booking != null)
            {
                _booking = _context.Bookings.Find(booking.BookingId);
                GuestComboBox.SelectedValue = _booking.GuestId;
                RoomCategoryComboBox.SelectedItem = _booking.Room.Category;
                CheckInDatePicker.SelectedDate = _booking.CheckInDate;
                CheckOutDatePicker.SelectedDate = _booking.CheckOutDate;
            }
            else
            {
                _booking = new Booking();
            }
        }

        private void SaveBooking_Click(object sender, RoutedEventArgs e)
        {
            if (GuestComboBox.SelectedValue == null || RoomCategoryComboBox.SelectedItem == null || 
                !CheckInDatePicker.SelectedDate.HasValue || !CheckOutDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Будь ласка, заповніть усі поля!");
                return;
            }

            var selectedCategory = RoomCategoryComboBox.SelectedItem.ToString();
            var availableRoom = _context.Rooms
                .Where(r => r.Category == selectedCategory && r.Status == "вільний")
                .OrderBy(r => r.RoomNumber)
                .FirstOrDefault();

            if (availableRoom == null)
            {
                MessageBox.Show("Немає доступних номерів цієї категорії!");
                return;
            }

            _booking.GuestId = (int)GuestComboBox.SelectedValue;
            _booking.RoomNumber = availableRoom.RoomNumber;
            _booking.CheckInDate = CheckInDatePicker.SelectedDate.Value;
            _booking.CheckOutDate = CheckOutDatePicker.SelectedDate.Value;
            _booking.Status = "підтверджено";

            if (_booking.BookingId == 0)
            {
                _context.Bookings.Add(_booking);
                availableRoom.Status = "заброньований";
            }
            else
            {
                _context.Entry(_booking).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            _context.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
