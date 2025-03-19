using System;
using System.Linq;
using System.Windows;
using HotelManagementSystem.Data;

namespace HotelManagementSystem
{
    public partial class RoomOccupancyReport : Window
    {
        private readonly HotelContext _context;

        public RoomOccupancyReport()
        {
            InitializeComponent();
            _context = new HotelContext();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Будь ласка, виберіть період!");
                return;
            }

            DateTime startDate = StartDatePicker.SelectedDate.Value;
            DateTime endDate = EndDatePicker.SelectedDate.Value;

            var occupancyData = _context.Bookings
                .Where(b => b.CheckInDate <= endDate && b.CheckOutDate >= startDate)
                .GroupBy(b => b.RoomNumber)
                .Select(g => new
                {
                    Номер = g.Key,
                    КількістьДнів = g.Sum(b => (b.CheckOutDate - b.CheckInDate).Days)
                })
                .OrderByDescending(o => o.КількістьДнів)
                .ToList();

            OccupancyDataGrid.ItemsSource = occupancyData;
        }
    }
}