using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogAnalizerWpfClient;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LogAnalizerShared;

public class LogApiClient
{
   
    private readonly HttpClient _httpClient;
  
    
    
    
    public bool IsAzureConnection =>
        _httpClient.BaseAddress?.Host.EndsWith("database.windows.net") == true;


    public LogApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task ImportLogsAsync(string filePath, LogWeekType weekType)
    {
        var response = await _httpClient.PostAsync(
            $"api/log/import?filePath={Uri.EscapeDataString(filePath)}&weekType={weekType}", null);

        response.EnsureSuccessStatusCode();
    }

   
  
    public async Task<List<ComparisonResult>> CompareWeeksInMemoryAsync(LogWeekType week1, LogWeekType week2)
    {
        return await _httpClient.GetFromJsonAsync<List<ComparisonResult>>(
            $"api/log/compare/result?week1={week1}&week2={week2}");
    }
    
    public async Task<List<LogWeekType>> GetAvailableWeekTypesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<LogWeekType>>("api/log/available-weeks");
    }
    public async Task<List<AlarmlogClient>> GetLogsByWeekAsync(LogWeekType week)
    {
        return await _httpClient.GetFromJsonAsync<List<AlarmlogClient>>($"api/log/logs-by-week?week={week}");
    }
    
    
    public async Task<List<string>> GetDatabasesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<string>>("api/database/list");
    }

    public async Task CreateDatabaseAsync(string dbName)
    {
        var requestUri = $"api/database/create?dbName={Uri.EscapeDataString(dbName)}";
        var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task SelectDatabaseAsync(string dbName)
    {
        var response = await _httpClient.PostAsync($"api/database/select?dbName={Uri.EscapeDataString(dbName)}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteDatabaseAsync(string dbName)
    {
        var response = await _httpClient.DeleteAsync($"api/database/delete?dbName={Uri.EscapeDataString(dbName)}");
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<List<ComparisonResult>> CompareByDateRangeAsync(DateTimeRangeComparisonRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/log/compare/by-daterange", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ComparisonResult>>();
    }
    
    public async Task<(DateTime Min, DateTime Max)> GetMinMaxGenerationTimeAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<MinMaxGenerationTimeDto>("api/log/min-max-generationtime");
        return (result.Min, result.Max);
    }
    
    public async Task SetServerNameAsync(string serverName)
    {
        var content = new StringContent($"\"{serverName}\"", System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/database/set-server", content);
        response.EnsureSuccessStatusCode();
    }
    
    
    public async Task SelectSqlServerDatabaseAsync(string server, string db, string user, string password)
    {
        var payload = new { server, db, user, password };
        var response = await _httpClient.PostAsJsonAsync("api/database/select-sqlserver", payload);
        response.EnsureSuccessStatusCode();
    }

   


    public async Task SetSqlServerAsync(string serverName)
    {
        var content = new StringContent($"\"{serverName}\"", Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/database/set-server", content);
        response.EnsureSuccessStatusCode();


    }
    
    
    public async Task SelectSqliteDatabaseByNameAsync(string fileName)
    {
        var payload = new SqliteRequest { FilePath = fileName };
        var response = await _httpClient.PostAsJsonAsync("api/database/select-sqlite", payload);
        response.EnsureSuccessStatusCode();
    }
    
    
    public async Task CreateSqliteDatabaseAsync(string fileName)
    {
        var payload = new SqliteRequest { FilePath = fileName };
        var response = await _httpClient.PostAsJsonAsync("api/database/create-sqlite", payload);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteSqliteDatabaseAsync(string fileName)
    {
        var payload = new SqliteRequest { FilePath = fileName };
        var response = await _httpClient.PostAsJsonAsync("api/database/delete-sqlite", payload);
        response.EnsureSuccessStatusCode();
    }
    
    
    public async Task<List<string>> GetSqliteDatabasesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<string>>("api/database/list-sqlite");
    }
    
    
    public async Task SetSqlServerWithAuthAsync(SqlServerAuthRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/database/set-server-auth", request);
        response.EnsureSuccessStatusCode();

       
    }
    
   
    
    
    
    
}