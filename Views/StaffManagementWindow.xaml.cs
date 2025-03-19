using System.Linq;
using System.Windows;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem
{
    public partial class StaffWindow : Window
    {
        private readonly HotelContext _context;

        public StaffWindow()
        {
            InitializeComponent();
            _context = new HotelContext();
            LoadStaff();
        }

        private void LoadStaff()
        {
            StaffDataGrid.ItemsSource = _context.Staff.ToList();
        }

        private void AddStaff_Click(object sender, RoutedEventArgs e)
        {
            var addStaffWindow = new AddEditStaffWindow();
            if (addStaffWindow.ShowDialog() == true)
            {
                LoadStaff();
            }
        }

        private void EditStaff_Click(object sender, RoutedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem is Staff selectedStaff)
            {
                var editStaffWindow = new AddEditStaffWindow(selectedStaff);
                if (editStaffWindow.ShowDialog() == true)
                {
                    LoadStaff();
                }
            }
            else
            {
                MessageBox.Show("Оберіть працівника для редагування.");
            }
        }

        private void DeleteStaff_Click(object sender, RoutedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem is Staff selectedStaff)
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити працівника?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Staff.Remove(selectedStaff);
                    _context.SaveChanges();
                    LoadStaff();
                }
            }
            else
            {
                MessageBox.Show("Оберіть працівника для видалення.");
            }
        }

        private void AssignCleaning_Click(object sender, RoutedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem is Staff selectedStaff)
            {
                var assignCleaningWindow = new AssignCleaningWindow(selectedStaff);
                if (assignCleaningWindow.ShowDialog() == true)
                {
                    // Refresh if needed
                }
            }
            else
            {
                MessageBox.Show("Оберіть працівника для призначення прибирання.");
            }
        }
    }
}