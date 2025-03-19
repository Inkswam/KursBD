using System;
using System.Windows;
using System.Windows.Controls;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem
{
    public partial class AddEditRoomWindow : Window
    {
        private readonly HotelContext _context;
        private Room _room;
        private bool _isEditing = false;

        public AddEditRoomWindow(Room room = null)
        {
            InitializeComponent();
            _context = new HotelContext();

            if (room != null)
            {
                _isEditing = true;
                _room = _context.Rooms.Find(room.RoomNumber);
                
                // Disable room number editing for existing rooms
                RoomNumberTextBox.Text = _room.RoomNumber.ToString();
                RoomNumberTextBox.IsEnabled = false;
                
                PriceTextBox.Text = _room.PricePerNight.ToString();
                
                // Set selected category
                foreach (var item in CategoryComboBox.Items)
                {
                    if (((ComboBoxItem)item).Content.ToString() == _room.Category)
                    {
                        CategoryComboBox.SelectedItem = item;
                        break;
                    }
                }
                
                // Set selected status
                foreach (var item in StatusComboBox.Items)
                {
                    if (((ComboBoxItem)item).Content.ToString() == _room.Status)
                    {
                        StatusComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                _room = new Room();
            }
        }

        private void SaveRoom_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RoomNumberTextBox.Text) || 
                CategoryComboBox.SelectedItem == null || 
                StatusComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля!");
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Будь ласка, введіть коректну ціну!");
                return;
            }

            if (!_isEditing)
            {
                if (!int.TryParse(RoomNumberTextBox.Text, out int roomNumber))
                {
                    MessageBox.Show("Номер кімнати повинен бути числом!");
                    return;
                }

                // Check if room number already exists
                var existingRoom = _context.Rooms.Find(roomNumber);
                if (existingRoom != null)
                {
                    MessageBox.Show($"Номер кімнати {roomNumber} вже існує!");
                    return;
                }

                _room.RoomNumber = roomNumber.ToString();  // Store as string if necessary
            }

            _room.Category = ((ComboBoxItem)CategoryComboBox.SelectedItem).Content.ToString();
            _room.Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString();
            _room.PricePerNight = price;

            if (!_isEditing)
                _context.Rooms.Add(_room);
            else
                _context.Entry(_room).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
