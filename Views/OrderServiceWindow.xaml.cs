using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem
{
    public partial class OrderServiceWindow : Window
    {
        private Service _selectedService;
        private HotelContext _dbContext;
        
        public OrderServiceWindow(Service selectedService)
        {
            InitializeComponent();
            
            _selectedService = selectedService;
            _dbContext = new HotelContext();
            
            // Set service information
            ServiceNameTextBlock.Text = _selectedService.Name;
            ServicePriceTextBlock.Text = _selectedService.Price.ToString("C");
            
            LoadGuests();
        }
        
        private void LoadGuests()
        {
            try
            {
                // Load active guests from database using Entity Framework
                List<Guest> guests = _dbContext.Guests
                    .Where(g => g.Bookings.Any(b => b.CheckOutDate >= DateTime.Today))
                    .ToList();
                
                GuestComboBox.ItemsSource = guests;
                GuestComboBox.DisplayMemberPath = "FullName";
                GuestComboBox.SelectedValuePath = "GuestId";
                
                if (guests.Count > 0)
                    GuestComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні списку гостей: {ex.Message}", 
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void OrderService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GuestComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Будь ласка, виберіть гостя", 
                        "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                Guest selectedGuest = (Guest)GuestComboBox.SelectedItem;
                
                // Create new OrderedService entity
                OrderedService order = new OrderedService
                {
                    GuestId = selectedGuest.GuestId,
                    ServiceId = _selectedService.ServiceId,
                    OrderDate = DateTime.Now,
                    Price = _selectedService.Price
                };
                
                // Save the order to database
                _dbContext.OrderedServices.Add(order);
                _dbContext.SaveChanges();
                
                MessageBox.Show($"Послугу \"{_selectedService.Name}\" успішно замовлено для гостя {selectedGuest.FullName}", 
                    "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при оформленні замовлення: {ex.Message}", 
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            // Dispose the db context when the window is closed
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }
    }
}