using System.Linq;
using System.Windows;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem
{
    public partial class ServicesWindow : Window
    {
        private readonly HotelContext _context;

        public ServicesWindow()
        {
            InitializeComponent();
            _context = new HotelContext();
            LoadServices();
        }

        private void LoadServices()
        {
            ServicesDataGrid.ItemsSource = _context.Services.ToList();
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            var addServiceWindow = new AddEditServiceWindow();
            if (addServiceWindow.ShowDialog() == true)
            {
                LoadServices();
            }
        }

        private void EditService_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesDataGrid.SelectedItem is Service selectedService)
            {
                var editServiceWindow = new AddEditServiceWindow(selectedService);
                if (editServiceWindow.ShowDialog() == true)
                {
                    LoadServices();
                }
            }
            else
            {
                MessageBox.Show("Оберіть послугу для редагування.");
            }
        }

        private void DeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesDataGrid.SelectedItem is Service selectedService)
            {
                // Check if service has been ordered
                var hasOrders = _context.OrderedServices.Any(o => o.ServiceId == selectedService.ServiceId);
                if (hasOrders)
                {
                    MessageBox.Show("Ця послуга вже замовлена гостями. Видалення неможливе.");
                    return;
                }

                if (MessageBox.Show("Ви впевнені, що хочете видалити цю послугу?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Services.Remove(selectedService);
                    _context.SaveChanges();
                    LoadServices();
                }
            }
            else
            {
                MessageBox.Show("Оберіть послугу для видалення.");
            }
        }

        private void OrderForGuest_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesDataGrid.SelectedItem is Service selectedService)
            {
                var orderServiceWindow = new OrderServiceWindow(selectedService);
                if (orderServiceWindow.ShowDialog() == true)
                {
                    // Refresh if needed
                }
            }
            else
            {
                MessageBox.Show("Оберіть послугу для замовлення.");
            }
        }
    }
}