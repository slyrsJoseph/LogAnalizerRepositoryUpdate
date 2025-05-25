
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogAnalizerServer.Data;
using LogAnalizerServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
namespace LogAnalizerServer

{
    using EFCore.BulkExtensions;
    public class LogService
    {
        private readonly ILogger<LogService> _logger;
        private readonly LogAnalizerServerDbContext _context;

        public LogService(LogAnalizerServerDbContext context, ILogger<LogService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
       

public async Task ImportLogsAsync(string filePath, LogWeekType weekType)
{
    using var stream = File.OpenRead(filePath);
    using var reader = new StreamReader(stream);

    string headerLine = await reader.ReadLineAsync();
    if (headerLine == null)
    {
        _logger.LogWarning("ImportLogsAsync: File Empty.");
        return;
    }

    int lineNumber = 1;
    string line;
    var logsToAdd = new List<AlarmLog>();

    while ((line = await reader.ReadLineAsync()) != null)
    {
        lineNumber++;
        if (string.IsNullOrWhiteSpace(line))
            continue;

        string[] fields = ParseCsvLine(line);
        if (fields.Length != 15)
        {
            _logger.LogWarning($"Line {lineNumber} has unexpected format");
            continue;
        }

        try
        {
            var alarmLog = new AlarmLog
            {
                TimeWhenLogged = DateTime.Parse(fields[0]),
                LocalZoneTime = DateTime.Parse(fields[1]),
                SequenceNumber = long.Parse(fields[2]),
                AlarmId = fields[3],
                AlarmClass = fields[4],
                Resource = fields[5],
                LoggedBy = fields[6],
                Reference = fields[7],
                PrevState = fields[8],
                LogAction = fields[9],
                FinalState = fields[10],
                AlarmMessage = fields[11],
                GenerationTime = DateTime.Parse(fields[12]),
                GenerationTimeUtc = DateTime.Parse(fields[13]),
                Project = fields[14],
                WeekType = weekType
            };

            logsToAdd.Add(alarmLog);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Line {lineNumber} has invalid data: {ex.Message}");
        }
    }

    if (logsToAdd.Any())
    {
        
        await _context.AlarmLogs
            .Where(log => log.WeekType == weekType)
            .BatchDeleteAsync();

        _logger.LogInformation($"Old logs *By weeks* deleted {weekType}");

       
      
        
        if (DatabaseConnectionManager.CurrentMode == DatabaseMode.Sqlite)
        {
            int nextId = 1;

            if (_context.AlarmLogs.Any())
                nextId = _context.AlarmLogs.Max(x => x.Id) + 1;

            foreach (var log in logsToAdd)
            {
                log.Id = nextId++;
            }
        }

        
        await _context.BulkInsertAsync(logsToAdd);
        _logger.LogInformation($"Imported {logsToAdd.Count} logs by BulkInsert.");
    }
}


        private string[] ParseCsvLine(string line)
        {
            List<string> fields = new List<string>();
            bool inQuotes = false;
            var field = new System.Text.StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '\"')
                {
                   
                    if (inQuotes && i < line.Length - 1 && line[i + 1] == '\"')
                    {
                        
                        field.Append('\"');
                        i++; 
                    }
                    else
                    {
                        
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    
                    fields.Add(field.ToString().Trim());
                    field.Clear();
                }
                else
                {
                    
                    field.Append(c);
                }
            }

          
            fields.Add(field.ToString().Trim());
            return fields.ToArray();
        }
      
       
        
        public async Task<List<ComparisonResult>> CompareWeeksInMemoryAsync(LogWeekType week1, LogWeekType week2)
        {
            
            var allowedAlarmClasses = new[]
            {
                "CRI_B", "CRI_C", "CRI_A", "FAULT",
                "SYS_A", "SYS_B", "SYS_C",
                "WRN", "WRN_A", "WRN_B", "WRN_C"
            };
            
            var week1Groups = await _context.AlarmLogs
                .Where(log => log.WeekType == week1 && log.FinalState == "G"&& allowedAlarmClasses.Contains(log.AlarmClass))
                .GroupBy(log => log.AlarmMessage)
                .Select(g => new { AlarmMessage = g.Key, Count = g.Count() })
                .ToListAsync();

            var week2Groups = await _context.AlarmLogs
                .Where(log => log.WeekType == week2 && log.FinalState == "G"&& allowedAlarmClasses.Contains(log.AlarmClass))
                .GroupBy(log => log.AlarmMessage)
                .Select(g => new { AlarmMessage = g.Key, Count = g.Count() })
                .ToListAsync();

            var week1Dict = week1Groups.ToDictionary(x => x.AlarmMessage, x => x.Count);
            var week2Dict = week2Groups.ToDictionary(x => x.AlarmMessage, x => x.Count);

            var allMessages = new HashSet<string>(week1Dict.Keys);
            allMessages.UnionWith(week2Dict.Keys);

            var comparisonResults = new List<ComparisonResult>();
            foreach (var message in allMessages)
            {
                week1Dict.TryGetValue(message, out int count1);
                week2Dict.TryGetValue(message, out int count2);

                comparisonResults.Add(new ComparisonResult
                {
                    AlarmMessage = message,
                    CountWeek1 = count1,
                    CountWeek2 = count2,
                    Week1Type = week1,
                    Week2Type = week2
                });
            }

            return comparisonResults;
        }

        
        public async Task<List<LogWeekType>> GetAvailableWeekTypesAsync()
        {
            return await _context.AlarmLogs
                .Select(log => log.WeekType)
                .Distinct()
                .ToListAsync();
        }
        
        public async Task<List<AlarmLog>> GetLogsByWeekAsync(LogWeekType week)
        {
            return await _context.AlarmLogs
                .Where(log => log.WeekType == week)
                .ToListAsync();
        }
        
    }
}
