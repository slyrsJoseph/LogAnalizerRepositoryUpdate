using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LogAnalizerShared; // Для доступа к DatabaseMode

namespace LogAnalizerWpfClient
{
    public partial class DatabaseManagerWindow : MaterialDesignWindow
    {
        private readonly LogApiClient _logApiClient;
        private readonly DatabaseMode _mode;

        public DatabaseManagerWindow(LogApiClient logApiClient, DatabaseMode mode)
        {
            InitializeComponent();
            _logApiClient = logApiClient;
            _mode = mode;
            LoadDatabases();
        }

        private async void LoadDatabases()
        {
            try
            {
                List<string> databases;

                if (_mode == DatabaseMode.Sqlite)
                    databases = await _logApiClient.GetSqliteDatabasesAsync();
                else
                    databases = await _logApiClient.GetDatabasesAsync();

                comboDatabases.ItemsSource = databases;
                if (databases.Any())
                    comboDatabases.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading DB list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dbName = txtNewDatabase.Text.Trim();
            if (string.IsNullOrWhiteSpace(dbName))
            {
                MessageBox.Show("Enter database name.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_mode == DatabaseMode.Sqlite)
                {
                    await _logApiClient.CreateSqliteDatabaseAsync(dbName);
                }
                else
                {
                    await _logApiClient.CreateDatabaseAsync(dbName);
                }

                MessageBox.Show("Database created.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDatabases();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnUse_Click(object sender, RoutedEventArgs e)
        {
            if (comboDatabases.SelectedItem == null)
            {
                MessageBox.Show("Select database.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dbName = comboDatabases.SelectedItem.ToString();

            try
            {
                if (_mode == DatabaseMode.Sqlite)
                    await _logApiClient.SelectSqliteDatabaseByNameAsync(dbName);
                else
                    await _logApiClient.SelectDatabaseAsync(dbName);
                
                
                SelectedDatabaseState.Set(dbName);


                MessageBox.Show("Database selected.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (comboDatabases.SelectedItem == null)
            {
                MessageBox.Show("Select database to remove.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dbName = comboDatabases.SelectedItem.ToString();

            var result = MessageBox.Show($"Are you sure you want to delete '{dbName}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                if (_mode == DatabaseMode.Sqlite)
                    await _logApiClient.DeleteSqliteDatabaseAsync(dbName);
                else
                    await _logApiClient.DeleteDatabaseAsync(dbName);

                MessageBox.Show("Database removed.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDatabases();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        
        private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        private void Close_Click(object sender, RoutedEventArgs e) => Close();
        
        
        
        
    }
}