using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem
{
    public partial class AssignCleaningWindow : Window
    {
        private readonly HotelContext _context;
        private readonly Staff _staff;

        public AssignCleaningWindow(Staff staff)
        {
            InitializeComponent();
            _context = new HotelContext();
            _staff = staff;

            StaffNameTextBlock.Text = _staff.FullName;
            CleaningDatePicker.SelectedDate = DateTime.Today;

            // Заповнення списку номерів
            var rooms = _context.Rooms.ToList();
            foreach (var room in rooms)
            {
                RoomComboBox.Items.Add(new ComboBoxItem { Content = $"Номер {room.RoomNumber} ({room.Category})", Tag = room });
            }

            if (RoomComboBox.Items.Count > 0)
                RoomComboBox.SelectedIndex = 0;

            if (CleaningTimeComboBox.Items.Count > 0)
                CleaningTimeComboBox.SelectedIndex = 0;

            if (CleaningTypeComboBox.Items.Count > 0)
                CleaningTypeComboBox.SelectedIndex = 0;
        }

        private void AssignCleaning_Click(object sender, RoutedEventArgs e)
        {
            if (RoomComboBox.SelectedItem == null || CleaningDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Будь ласка, виберіть номер та дату прибирання!");
                return;
            }

            var selectedRoom = ((ComboBoxItem)RoomComboBox.SelectedItem).Tag as Room;
            var selectedDate = CleaningDatePicker.SelectedDate.Value;
            var selectedTime = ((ComboBoxItem)CleaningTimeComboBox.SelectedItem).Content.ToString();
            var selectedType = ((ComboBoxItem)CleaningTypeComboBox.SelectedItem).Content.ToString();

            // Створення нового завдання з прибирання
            var cleaning = new Cleaning
            {
                RoomNumber = selectedRoom.RoomNumber,  // Ensure RoomNumber is an int
                StaffId = _staff.StaffId,
                CleaningDate = selectedDate,
                CleaningTime = selectedTime,
                CleaningType = selectedType,
                Status = "Заплановано"
            };

            _context.Cleanings.Add(cleaning);

            // Зміна статусу номера на "прибирання" на час прибирання
            selectedRoom.Status = "прибирання";
            _context.Entry(selectedRoom).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context.SaveChanges();

            MessageBox.Show("Прибирання успішно призначено!");
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
