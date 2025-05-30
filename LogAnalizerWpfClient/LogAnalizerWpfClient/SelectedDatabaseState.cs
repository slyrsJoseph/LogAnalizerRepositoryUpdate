namespace LogAnalizerWpfClient;

public static class SelectedDatabaseState
{
    public static string CurrentDatabaseName { get; private set; } = string.Empty;

    public static void Set(string dbName)
    {
        CurrentDatabaseName = dbName;
    }
}