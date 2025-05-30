using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LogAnalizerShared;

namespace LogAnalizerWpfClient
{
    public partial class TableDateTimeComparisonWindow  : MaterialDesignWindow
    {
        private readonly LogApiClient _apiClient;
        private List<AlarmlogClient> _allLogs = new();

        private readonly List<string> _equipmentCategories = new()
        {
            "All", "VPH", "BRC", "LGA", "HRN", "DDM", "DW",
            "DW VFD", "DW ZPS", "DW ECS", "DW ADS", "TFM", "ELT", "PDPH"
        };

        private readonly string[] _allowedAlarmClasses =
        {
            "CRI_B", "CRI_C", "CRI_A", "FAULT",
            "SYS_A", "SYS_B", "SYS_C",
            "WRN", "WRN_A", "WRN_B", "WRN_C"
        };

        public TableDateTimeComparisonWindow(LogApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            comboEquipment.ItemsSource = _equipmentCategories;
            comboEquipment.SelectedIndex = 0;
            LoadAvailableWeeks();
        }

        private async void LoadAvailableWeeks()
        {
            var weeks = await _apiClient.GetAvailableWeekTypesAsync();
            comboWeek.ItemsSource = weeks;
            comboWeek.SelectedIndex = 0;
        }

        private async void comboWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboWeek.SelectedItem is not LogWeekType week) return;

            _allLogs = await _apiClient.GetLogsByWeekAsync(week);
            LoadFilteredLogs();
        }

        private void checkFilter_Changed(object sender, RoutedEventArgs e)
        {
            LoadFilteredLogs();
        }

        private void comboEquipment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadFilteredLogs();
        }

        private void LoadFilteredLogs()
        {
            if (_allLogs == null || !_allLogs.Any()) return;

            string selectedCategory = comboEquipment.SelectedItem?.ToString() ?? "All";
            var filtered = _allLogs;

            if (checkFilter.IsChecked == true)
            {
                filtered = filtered
                    .Where(log => log.FinalState == "G" && _allowedAlarmClasses.Contains(log.AlarmClass))
                    .ToList();
            }

            if (selectedCategory != "All")
            {
                filtered = filtered
                    .Where(log => log.AlarmMessage.Contains(selectedCategory, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            dataGridLogs.ItemsSource = filtered;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        
        
        
        private void checkFilter_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredLogs();
        }

        private void checkFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredLogs();
        }
        
        
        
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            SelectedDbText.Text = $"Selected DB: {SelectedDatabaseState.CurrentDatabaseName}";
        }
        
    }
}
