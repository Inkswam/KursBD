using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using HotelManagementSystem.Data;

namespace HotelManagementSystem
{
    public partial class RevenueReportWindow : Window
    {
        private readonly HotelContext _context;

        public RevenueReportWindow()
        {
            InitializeComponent();
            _context = new HotelContext();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Будь ласка, виберіть період!");
                return;
            }

            DateTime startDate = StartDatePicker.SelectedDate.Value;
            DateTime endDate = EndDatePicker.SelectedDate.Value;

            var revenueData = _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .Select(p => new
                {
                    Дата = p.PaymentDate,
                    Сума = p.Amount,
                    СпосібОплати = p.PaymentMethod
                })
                .ToList();

            RevenueDataGrid.ItemsSource = revenueData;

            decimal totalRevenue = revenueData.Sum(r => r.Сума);
            TotalRevenueTextBlock.Text = $"{totalRevenue:C}";
        }
        private void ExportToCsv_Click(object sender, RoutedEventArgs e)
        {
            if (RevenueDataGrid.ItemsSource == null || !RevenueDataGrid.ItemsSource.Cast<object>().Any())
            {
                MessageBox.Show("Немає даних для експорту!");
                return;
            }

            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Дата,Сума,СпосібОплати");

            foreach (var item in RevenueDataGrid.ItemsSource.Cast<dynamic>())
            {
                csv.AppendLine($"{item.Дата},{item.Сума},{item.СпосібОплати}");
            }

            File.WriteAllText("RevenueReport.csv", csv.ToString(), Encoding.UTF8);
            MessageBox.Show("Звіт збережено у файл RevenueReport.csv!");
        }
        
    }
}