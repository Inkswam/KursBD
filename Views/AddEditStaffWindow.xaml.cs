using System.Windows;
using System.Windows.Controls;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem
{
    public partial class AddEditStaffWindow : Window
    {
        private readonly HotelContext _context;
        private Staff _staff;

        public AddEditStaffWindow(Staff staff = null)
        {
            InitializeComponent();
            _context = new HotelContext();

            if (staff != null)
            {
                _staff = _context.Staff.Find(staff.StaffId);
                FullNameTextBox.Text = _staff.FullName;
                
                // Set selected position
                foreach (var item in PositionComboBox.Items)
                {
                    if (((ComboBoxItem)item).Content.ToString() == _staff.Position)
                    {
                        PositionComboBox.SelectedItem = item;
                        break;
                    }
                }
                
                // Set selected schedule
                foreach (var item in WorkScheduleComboBox.Items)
                {
                    if (((ComboBoxItem)item).Content.ToString() == _staff.WorkSchedule)
                    {
                        WorkScheduleComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                _staff = new Staff();
            }
        }

        private void SaveStaff_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) || 
                PositionComboBox.SelectedItem == null || 
                WorkScheduleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, заповніть усі поля!");
                return;
            }

            _staff.FullName = FullNameTextBox.Text;
            _staff.Position = ((ComboBoxItem)PositionComboBox.SelectedItem).Content.ToString();
            _staff.WorkSchedule = ((ComboBoxItem)WorkScheduleComboBox.SelectedItem).Content.ToString();

            if (_staff.StaffId == 0)
                _context.Staff.Add(_staff);
            else
                _context.Entry(_staff).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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