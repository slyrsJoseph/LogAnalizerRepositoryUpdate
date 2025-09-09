using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace LogAnalizerWpfClient;

public partial class ExportSqliteDialog : MaterialDesignWindow
{
    
    public string SqliteFilePath => txtSqliteFilePath.Text.Trim();
    public string Server => txtServer.Text.Trim();
    public string Database => txtDatabase.Text.Trim();
    public string Username => txtUsername.Text.Trim();
    public string Password => txtPassword.Password;

    public ExportSqliteDialog()
    {
        InitializeComponent();
    }
    
    
    
    private void BrowseSqliteFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "SQLite files (*.sqlite)|*.sqlite|All files (*.*)|*.*",
            Title = "Select SQLite file"
        };

        if (dialog.ShowDialog() == true)
        {
            txtSqliteFilePath.Text = dialog.FileName;
        }
    }
    

    private void Export_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Server) ||
            string.IsNullOrWhiteSpace(Database) ||
            string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Password))
        {
            MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
    
    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
    private void Close_Click(object sender, RoutedEventArgs e) => Close();
    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }
    
    
}
