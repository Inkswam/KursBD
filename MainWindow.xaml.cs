using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem
{
    public partial class MainWindow : Window
    {
        private readonly HotelContext _context;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _context = new HotelContext();
            
            // Initialize timer for updating date/time
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            
            // Load initial data
            LoadGuests();
            LoadBookings();
            UpdateDashboardStatistics();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTimeTextBlock.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        private void LoadGuests()
        {
            GuestsDataGrid.ItemsSource = _context.Guests.ToList();
        }

        private void LoadBookings()
        {
            BookingsDataGrid.ItemsSource = _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .ToList();
        }

        private void UpdateDashboardStatistics()
        {
            try
            {
                // Update total guests
                int totalGuests = _context.Guests.Count();
                TotalGuestsTextBlock.Text = totalGuests.ToString();

                // Update occupied rooms
                int occupiedRooms = _context.Rooms.Count(r => r.Status == "заброньований" || r.Status == "зайнятий");
                OccupiedRoomsTextBlock.Text = occupiedRooms.ToString();

                // Update active bookings
                int activeBookings = _context.Bookings.Count(b => b.CheckOutDate >= DateTime.Today);
                ActiveBookingsTextBlock.Text = activeBookings.ToString();

                // Update current month revenue
                DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                decimal currentMonthRevenue = _context.Payments
                    .Where(p => p.PaymentDate >= firstDayOfMonth && p.PaymentDate <= lastDayOfMonth)
                    .Sum(p => p.Amount);
                CurrentMonthRevenueTextBlock.Text = currentMonthRevenue.ToString("C");

                StatusTextBlock.Text = "Статистика оновлена: " + DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Помилка оновлення статистики: " + ex.Message;
            }
        }

        #region Guest Management
        private void GuestsManagement_Click(object sender, RoutedEventArgs e)
        {
            // We're already on the main window, just switch to the Guests tab
            TabControl mainTabControl = FindName("TabControl") as TabControl;
            if (mainTabControl != null)
            {
                mainTabControl.SelectedIndex = 0; // Assume Guests tab is first
            }
        }

        private void AddGuest_Click(object sender, RoutedEventArgs e)
        {
            var addGuestWindow = new AddGuestWindow();
            if (addGuestWindow.ShowDialog() == true)
            {
                LoadGuests();
                UpdateDashboardStatistics();
            }
        }

        private void EditGuest_Click(object sender, RoutedEventArgs e)
        {
            if (GuestsDataGrid.SelectedItem is Guest selectedGuest)
            {
                var editGuestWindow = new AddGuestWindow(selectedGuest);
                if (editGuestWindow.ShowDialog() == true)
                {
                    LoadGuests();
                    UpdateDashboardStatistics();
                }
            }
            else
            {
                MessageBox.Show("Оберіть гостя для редагування.");
            }
        }

        private void DeleteGuest_Click(object sender, RoutedEventArgs e)
        {
            if (GuestsDataGrid.SelectedItem is Guest selectedGuest)
            {
                // Check if guest has any bookings
                bool hasBookings = _context.Bookings.Any(b => b.GuestId == selectedGuest.GuestId);
                if (hasBookings)
                {
                    MessageBox.Show("Неможливо видалити гостя, оскільки він має бронювання.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (MessageBox.Show("Ви впевнені, що хочете видалити гостя?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Guests.Remove(selectedGuest);
                    _context.SaveChanges();
                    LoadGuests();
                    UpdateDashboardStatistics();
                }
            }
            else
            {
                MessageBox.Show("Оберіть гостя для видалення.");
            }
        }
        #endregion

        #region Booking Management
        private void BookingsManagement_Click(object sender, RoutedEventArgs e)
        {
            // We're already on the main window, just switch to the Bookings tab
            TabControl mainTabControl = FindName("TabControl") as TabControl;
            if (mainTabControl != null)
            {
                mainTabControl.SelectedIndex = 1; // Assume Bookings tab is second
            }
        }

        private void AddBooking_Click(object sender, RoutedEventArgs e)
        {
            var addBookingWindow = new AddBookingWindow();
            if (addBookingWindow.ShowDialog() == true)
            {
                LoadBookings();
                UpdateDashboardStatistics();
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
                    UpdateDashboardStatistics();
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
                // Check if booking has payments
                bool hasPayments = _context.Payments.Any(p => p.BookingId == selectedBooking.BookingId);
                if (hasPayments)
                {
                    MessageBox.Show("Неможливо видалити бронювання, оскільки воно має платежі.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (MessageBox.Show("Ви впевнені, що хочете видалити бронювання?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // Update room status
                    var room = _context.Rooms.Find(selectedBooking.RoomNumber);
                    if (room != null)
                    {
                        room.Status = "вільний";
                    }

                    _context.Bookings.Remove(selectedBooking);
                    _context.SaveChanges();
                    LoadBookings();
                    UpdateDashboardStatistics();
                }
            }
            else
            {
                MessageBox.Show("Оберіть бронювання для видалення.");
            }
        }
        #endregion

        #region Payment and Reports
        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            var paymentWindow = new PaymentWindow();
            if (paymentWindow.ShowDialog() == true)
            {
                LoadBookings();
                UpdateDashboardStatistics();
            }
        }

        private void RevenueReport_Click(object sender, RoutedEventArgs e)
        {
            var revenueReportWindow = new RevenueReportWindow();
            revenueReportWindow.ShowDialog();
        }

        private void OccupancyReport_Click(object sender, RoutedEventArgs e)
        {
            var roomOccupancyReport = new RoomOccupancyReport();
            roomOccupancyReport.ShowDialog();
        }
        #endregion

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ви впевнені, що хочете вийти з програми?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _timer.Stop();
            _context.Dispose();
            base.OnClosing(e);
        }
        
        // Method to refresh all data
        private void RefreshAllData()
        {
            LoadGuests();
            LoadBookings();
            UpdateDashboardStatistics();
            StatusTextBlock.Text = "Дані оновлено: " + DateTime.Now.ToString("HH:mm:ss");
        }
    }
}