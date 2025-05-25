using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using LogAnalizerShared;
using Microsoft.Win32;
using System.Data;
using System.Data.Sql;

namespace LogAnalizerWpfClient
{
    public partial class MainWindow : Window
    {
        private readonly LogApiClient _logApiClient;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            _logApiClient = new LogApiClient(new HttpClient { BaseAddress = new Uri("http://localhost:5001") });
           
           var httpClient = new HttpClient
           {
               BaseAddress = new Uri("http://localhost:5001"),
               Timeout = TimeSpan.FromMinutes(5) 
           };

           _logApiClient = new LogApiClient(httpClient);
           
           
        }

        private void btnManageDatabases_Click(object sender, RoutedEventArgs e)
        {
            string selectedMode = (comboDbMode.SelectedItem as ComboBoxItem)?.Content.ToString();
            var mode = selectedMode == "SQLite" ? DatabaseMode.Sqlite : DatabaseMode.SqlServer;

            var window = new DatabaseManagerWindow(_logApiClient, mode);
            window.ShowDialog();
        }

        private void btnCompareByDateRange_Click(object sender, RoutedEventArgs e)
        {
            var window = new DateTimeComparisonWindow(_logApiClient);
            window.ShowDialog();
        }

        private void btnWeekComparison_Click(object sender, RoutedEventArgs e)
        {
            var window = new WeekByWeekComparisonWindow(_logApiClient);
            window.ShowDialog();
        }

        private void btnOpenTableDateRange_Click(object sender, RoutedEventArgs e)
        {
            var window = new TableDateTimeComparisonWindow(_logApiClient);
            window.Show();
        }

        private void comboDbMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = (comboDbMode.SelectedItem as ComboBoxItem)?.Content.ToString();

            sqlServerPanel.Visibility = selected == "SQL Server" ? Visibility.Visible : Visibility.Collapsed;
            btnManageDatabases.IsEnabled = selected == "SQLite";
        }

        private void comboAuthMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (comboAuthMode.SelectedItem as ComboBoxItem)?.Content.ToString();
            sqlAuthFields.Visibility = selected == "SQL Authentication" ? Visibility.Visible : Visibility.Collapsed;
        }

     private async void btnConnect_Click(object sender, RoutedEventArgs e)
{
    string serverName = txtServerName.Text.Trim();
    if (string.IsNullOrWhiteSpace(serverName))
    {
        MessageBox.Show("Enter server name.");
        return;
    }

    var authMode = (comboAuthMode.SelectedItem as ComboBoxItem)?.Content.ToString();

    if (authMode == "SQL Authentication")
    {
        var login = txtLogin.Text.Trim();
        var password = txtPassword.Password;

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Enter login and password.");
            return;
        }

        var request = new SqlServerAuthRequest
        {
            Server = serverName,
            User = login,
            Password = password
        };

        try
        {
            await _logApiClient.SetSqlServerWithAuthAsync(request);
            MessageBox.Show("Connected to remote server", "Success");
            btnManageDatabases.IsEnabled = true;
        }
        catch (HttpRequestException ex)
        {
            string errorMessage = ex.Message;
            if (ex.Data["Response"] is HttpResponseMessage response)
            {
                errorMessage = await response.Content.ReadAsStringAsync();
            }
            MessageBox.Show("Connection Error: " + errorMessage);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Connection Error: " + ex.Message);
        }
    }
    else 
    {
        try
        {
            await _logApiClient.SetServerNameAsync(serverName);
            MessageBox.Show("Connected to SQL Server (Trusted Connection).", "Success");
            btnManageDatabases.IsEnabled = true;
        }
        catch (HttpRequestException ex)
        {
            string errorMessage = ex.Message;
            if (ex.Data["Response"] is HttpResponseMessage response)
            {
                errorMessage = await response.Content.ReadAsStringAsync();
            }
            MessageBox.Show("Connection Error: " + errorMessage);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Connection Error: " + ex.Message);
        }
    }
}
        

        private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        private void Close_Click(object sender, RoutedEventArgs e) => Close();
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
        
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        
        
    }
}