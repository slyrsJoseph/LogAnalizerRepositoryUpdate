using System.ComponentModel;

namespace LogAnalizerWpfClient;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private string _selectedDbName = "None";
    public string SelectedDbName
    {
        get => _selectedDbName;
        set
        {
            if (_selectedDbName != value)
            {
                _selectedDbName = value;
                OnPropertyChanged(nameof(SelectedDbName));
                OnPropertyChanged(nameof(IsDatabaseSelected));
            }
        }
    }

    public bool IsDatabaseSelected => SelectedDbName != "None";

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}