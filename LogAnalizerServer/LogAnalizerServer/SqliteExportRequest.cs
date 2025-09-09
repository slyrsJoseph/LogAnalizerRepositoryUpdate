namespace LogAnalizerServer;

public class SqliteExportRequest
{
    public string SqliteFile { get; set; }
    public string TargetServer { get; set; }
    public string TargetDatabase { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}