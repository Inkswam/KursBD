using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem
{
    public partial class ChangeRoomStatusWindow : Window
    {
        private readonly Room _room;
        private readonly HotelContext _context;

        public ChangeRoomStatusWindow(Room room)
        {
            InitializeComponent();

            _room = room;
            _context = new HotelContext();

            // Initialize window with room data
            RoomNumberTextBlock.Text = room.RoomNumber.ToString();  // Ensure it's treated as a string
            CurrentStatusTextBlock.Text = room.Status;

            // Load possible statuses into combobox
            List<string> statuses = new List<string>
            { 
                "Вільний", 
                "Зайнятий", 
                "На прибиранні", 
                "На обслуговуванні", 
                "Не доступний" 
            };

            StatusComboBox.ItemsSource = statuses;

            // Set the current status as selected
            if (statuses.Contains(room.Status))
            {
                StatusComboBox.SelectedItem = room.Status;
            }
            else
            {
                StatusComboBox.SelectedIndex = 0;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StatusComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Будь ласка, виберіть статус.", 
                        "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string newStatus = StatusComboBox.SelectedItem.ToString();

                // Check if current status is "Зайнятий" and trying to change to "Вільний"
                // This might require additional validation (e.g., checking if booking has ended)
                if (_room.Status == "Зайнятий" && newStatus == "Вільний")
                {
                    // Check if room has active bookings
                    bool hasActiveBookings = CheckForActiveBookings(_room.RoomNumber.ToString());  // Ensure RoomNumber is treated as a string
                    
                    if (hasActiveBookings)
                    {
                        var result = MessageBox.Show(
                            "Цей номер має активні бронювання. Ви впевнені, що хочете змінити статус на \"Вільний\"?",
                            "Підтвердження",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.No)
                        {
                            return;
                        }
                    }
                }

                // Update room status
                _room.Status = newStatus;
                _context.SaveChanges();

                MessageBox.Show($"Статус номера {_room.RoomNumber} успішно змінено на \"{newStatus}\".",
                    "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при зміні статусу номера: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckForActiveBookings(string roomNumber)
        {
            try
            {
                DateTime today = DateTime.Today;

                // Ensure you're comparing the same types: RoomNumber is of type int in the database
                return _context.Bookings.Any(b => 
                    b.RoomNumber.ToString() == roomNumber &&  // Make sure b.RoomNumber is converted to a string here
                    b.CheckOutDate >= today);
            }
            catch
            {
                // If there's an error checking for bookings, assume there are active bookings
                // to be on the safe side
                return true;
            }
        }

        public int RoomNumber { get; set; }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Dispose the db context when the window is closed
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
