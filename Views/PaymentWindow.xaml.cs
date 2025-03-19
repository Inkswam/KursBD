using System;
using System.Linq;
using System.Windows;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem
{
    public partial class PaymentWindow : Window
    {
        private readonly HotelContext _context;
        private Booking _selectedBooking;
        private decimal _totalAmount;

        public PaymentWindow()
        {
            InitializeComponent();
            _context = new HotelContext();
            LoadBookings();
        }

        private void LoadBookings()
        {
            var unpaidBookings = _context.Bookings
                .Where(b => !_context.Payments.Any(p => p.BookingId == b.BookingId))
                .ToList();

            BookingComboBox.ItemsSource = unpaidBookings;
            BookingComboBox.DisplayMemberPath = "BookingId";
            BookingComboBox.SelectedValuePath = "BookingId";
        }

        private void BookingComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (BookingComboBox.SelectedValue == null)
                return;

            int bookingId = (int)BookingComboBox.SelectedValue;
            _selectedBooking = _context.Bookings
                .Where(b => b.BookingId == bookingId)
                .FirstOrDefault();

            if (_selectedBooking != null)
            {
                _totalAmount = CalculateTotalAmount(_selectedBooking);
                TotalAmountTextBlock.Text = _totalAmount.ToString("C");
            }
        }

        private decimal CalculateTotalAmount(Booking booking)
        {
            int days = (booking.CheckOutDate - booking.CheckInDate).Days;
            decimal roomCost = booking.Room.PricePerNight * days;

            decimal servicesCost = _context.OrderedServices
                .Where(s => s.GuestId == booking.GuestId)
                .Sum(s => s.Price);

            return roomCost + servicesCost;
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBooking == null || PaymentMethodComboBox.SelectedItem == null)
            {
                MessageBox.Show("Оберіть бронювання та спосіб оплати!");
                return;
            }

            var payment = new Payment
            {
                BookingId = _selectedBooking.BookingId,
                Amount = _totalAmount,
                PaymentDate = DateTime.Now,
                PaymentMethod = PaymentMethodComboBox.Text
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            MessageBox.Show("Оплата успішно проведена!");

            GenerateReceipt(_selectedBooking, _totalAmount, payment.PaymentMethod);
            DialogResult = true;
            Close();
        }

        private void GenerateReceipt(Booking booking, decimal amount, string method)
        {
            string receipt = $"Квитанція про оплату\n\n" +
                             $"Бронювання ID: {booking.BookingId}\n" +
                             $"Гість: {booking.Guest.FullName}\n" +
                             $"Дата заїзду: {booking.CheckInDate.ToShortDateString()}\n" +
                             $"Дата виїзду: {booking.CheckOutDate.ToShortDateString()}\n" +
                             $"Сума: {amount:C}\n" +
                             $"Спосіб оплати: {method}\n" +
                             $"Дата платежу: {DateTime.Now}\n";

            System.IO.File.WriteAllText($"Receipt_{booking.BookingId}.txt", receipt);
            MessageBox.Show($"Квитанція збережена як Receipt_{booking.BookingId}.txt");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
