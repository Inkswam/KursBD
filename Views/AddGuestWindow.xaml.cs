using System.Windows;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem
{
    public partial class AddGuestWindow : Window
    {
        private readonly HotelContext _context;
        private Guest _guest;

        public AddGuestWindow(Guest guest = null)
        {
            InitializeComponent();
            _context = new HotelContext();

            if (guest != null)
            {
                _guest = _context.Guests.Find(guest.GuestId);
                FullNameTextBox.Text = _guest.FullName;
                ContactInfoTextBox.Text = _guest.ContactInfo;
            }
            else
            {
                _guest = new Guest();
            }
        }

        private void SaveGuest_Click(object sender, RoutedEventArgs e)
        {
            _guest.FullName = FullNameTextBox.Text;
            _guest.ContactInfo = ContactInfoTextBox.Text;

            if (_guest.GuestId == 0)
                _context.Guests.Add(_guest);
            else
                _context.Entry(_guest).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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