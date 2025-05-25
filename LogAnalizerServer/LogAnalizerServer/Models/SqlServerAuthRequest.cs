namespace LogAnalizerServer.Models;

public class SqlServerAuthRequest
{
    public string Server { get; set; } = "";
    public string Database { get; set; } = "";
    public string User { get; set; } = "";
    public string Password { get; set; } = "";
}