using System;
using System.Windows;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem
{
    public partial class AddEditServiceWindow : Window
    {
        private readonly HotelContext _context;
        private Service _service;

        public AddEditServiceWindow(Service service = null)
        {
            InitializeComponent();
            _context = new HotelContext();

            if (service != null)
            {
                _service = _context.Services.Find(service.ServiceId);
                NameTextBox.Text = _service.Name;
                DescriptionTextBox.Text = _service.Description;
                PriceTextBox.Text = _service.Price.ToString();
            }
            else
            {
                _service = new Service();
            }
        }

        private void SaveService_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || 
                string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Назва та ціна послуги обов'язкові!");
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Будь ласка, введіть коректну ціну!");
                return;
            }

            _service.Name = NameTextBox.Text;
            _service.Description = DescriptionTextBox.Text;
            _service.Price = price;

            if (_service.ServiceId == 0)
                _context.Services.Add(_service);
            else
                _context.Entry(_service).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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