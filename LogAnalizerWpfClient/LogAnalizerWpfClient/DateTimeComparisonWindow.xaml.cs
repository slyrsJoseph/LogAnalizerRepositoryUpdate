using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LogAnalizerShared;
using Microsoft.Win32;

namespace LogAnalizerWpfClient
{
    public partial class DateTimeComparisonWindow : Window
    {
        private readonly LogApiClient _apiClient;
        private Dictionary<LogWeekType, (DateTime Start, DateTime End)> _weekDateRanges;

        public DateTimeComparisonWindow(LogApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            LoadAvailableWeeks();
        }

        private async void LoadAvailableWeeks()
        {
            var weeks = await _apiClient.GetAvailableWeekTypesAsync();
            _weekDateRanges = new Dictionary<LogWeekType, (DateTime, DateTime)>();

            foreach (var week in weeks)
            {
                var logs = await _apiClient.GetLogsByWeekAsync(week);
                if (!logs.Any()) continue;

                var min = logs.Min(x => x.TimeWhenLogged);
                var max = logs.Max(x => x.TimeWhenLogged);

                _weekDateRanges[week] = (min, max);
            }

            comboWeek1.ItemsSource = _weekDateRanges.Keys;
            comboWeek2.ItemsSource = _weekDateRanges.Keys;
        }

        private void comboWeek1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboWeek1.SelectedItem is LogWeekType week && _weekDateRanges.TryGetValue(week, out var range))
            {
                dateRange1Start.SelectedDate = range.Start;
                dateRange1End.SelectedDate = range.End;
            }
        }

        private void comboWeek2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboWeek2.SelectedItem is LogWeekType week && _weekDateRanges.TryGetValue(week, out var range))
            {
                dateRange2Start.SelectedDate = range.Start;
                dateRange2End.SelectedDate = range.End;
            }
        }

        private async void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            if (dateRange1Start.SelectedDate is null || dateRange1End.SelectedDate is null ||
                dateRange2Start.SelectedDate is null || dateRange2End.SelectedDate is null)
            {
                MessageBox.Show("Select date ranges before comparision.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var request = new DateTimeRangeComparisonRequest
            {
                Range1Start = dateRange1Start.SelectedDate.Value,
                Range1End = dateRange1End.SelectedDate.Value,
                Range2Start = dateRange2Start.SelectedDate.Value,
                Range2End = dateRange2End.SelectedDate.Value
            };

            try
            {
                var results = await _apiClient.CompareByDateRangeAsync(request);

                if (results == null || !results.Any())
                {
                    MessageBox.Show("No data for date ranges selected.", "Information",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var chartWindow = new ChartDateTimeComparisonWindow(results, request, _apiClient);
                chartWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Comparision error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        
        
        
    }
}