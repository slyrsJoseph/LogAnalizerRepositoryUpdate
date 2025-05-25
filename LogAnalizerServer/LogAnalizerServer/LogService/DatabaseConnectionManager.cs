public enum DatabaseMode
{
    Sqlite,
    SqlServer
}

public static class DatabaseConnectionManager
{
    private static string _currentConnectionString = string.Empty;
    private static DatabaseMode _currentMode = DatabaseMode.SqlServer; 

    public static string CurrentConnectionString => _currentConnectionString;

    public static DatabaseMode CurrentMode => _currentMode;

   
    public static void UseSqlite(string databaseFilePath)
    {
        _currentMode = DatabaseMode.Sqlite;

        if (!Path.IsPathRooted(databaseFilePath))
        {
            databaseFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Databases", databaseFilePath);
        }

        _currentConnectionString = $"Data Source={databaseFilePath}";
    }

   
    public static void UseSqlServer(string server, string database, string user, string password)
    {
        _currentMode = DatabaseMode.SqlServer;

        _currentConnectionString = string.IsNullOrWhiteSpace(user)
            ? $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=True;"
            : $"Server={server};Database={database};User Id={user};Password={password};TrustServerCertificate=True;";
    }

    
    public static void SetConnectionString(string newConnectionString)
    {
        _currentMode = DatabaseMode.SqlServer;
        _currentConnectionString = newConnectionString;
    }

    
    public static void SetRawConnection(string connectionString, DatabaseMode mode)
    {
        _currentMode = mode;
        _currentConnectionString = connectionString;
    }
}