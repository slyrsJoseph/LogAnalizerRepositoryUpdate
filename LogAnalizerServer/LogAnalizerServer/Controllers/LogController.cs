using LogAnalizerServer.Data;
using LogAnalizerServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogAnalizerServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogController : ControllerBase
{
    private readonly LogService _logService;

    public LogController(LogService logService)
    {
        _logService = logService;
    }

   
    [HttpPost("import")]
    public async Task<IActionResult> ImportLogs([FromQuery] string filePath, [FromQuery] LogWeekType weekType)
    {
        await _logService.ImportLogsAsync(filePath, weekType);
        return Ok("Import finished successful.");
    }

 
    [HttpGet("compare/result")]
    public async Task<IActionResult> CompareAndReturnResult([FromQuery] LogWeekType week1,
        [FromQuery] LogWeekType week2)
    {
        var result = await _logService.CompareWeeksInMemoryAsync(week1, week2);
        return Ok(result);
    }

    
    [HttpGet("available-weeks")]
    public async Task<IActionResult> GetAvailableWeeks()
    {
        var weeks = await _logService.GetAvailableWeekTypesAsync();
        return Ok(weeks);
    }

    
    [HttpGet("logs-by-week")]
    public async Task<IActionResult> GetLogsByWeek([FromQuery] LogWeekType week)
    {
        var logs = await _logService.GetLogsByWeekAsync(week);
        return Ok(logs);
    }
    
[HttpPost("compare/by-daterange")]
public async Task<ActionResult<List<ComparisonResult>>> CompareByDateRange([FromBody] DateTimeRangeComparisonRequest request)
{
    try
    {
        var optionsBuilder = new DbContextOptionsBuilder<LogAnalizerServerDbContext>();

        if (DatabaseConnectionManager.CurrentMode == DatabaseMode.Sqlite)
            optionsBuilder.UseSqlite(DatabaseConnectionManager.CurrentConnectionString);
        else
            optionsBuilder.UseSqlServer(DatabaseConnectionManager.CurrentConnectionString);

        await using var context = new LogAnalizerServerDbContext(optionsBuilder.Options);

        var allowedAlarmClasses = new[]
        {
            "CRI_B", "CRI_C", "CRI_A", "FAULT",
            "SYS_A", "SYS_B", "SYS_C",
            "WRN", "WRN_A", "WRN_B", "WRN_C","B","C"
        };

        var range1Logs = await context.AlarmLogs
            .Where(log =>
                log.GenerationTime >= request.Range1Start &&
                log.GenerationTime <= request.Range1End &&
                log.FinalState == "G" &&
                allowedAlarmClasses.Contains(log.AlarmClass))
            .ToListAsync();

        var range2Logs = await context.AlarmLogs
            .Where(log =>
                log.GenerationTime >= request.Range2Start &&
                log.GenerationTime <= request.Range2End &&
                log.FinalState == "G" &&
                allowedAlarmClasses.Contains(log.AlarmClass))
            .ToListAsync();

        if (!range1Logs.Any() || !range2Logs.Any())
        {
            return BadRequest("No logs in range selected.");
        }

        var grouped1 = range1Logs.GroupBy(l => l.AlarmMessage)
            .Select(g => new { AlarmMessage = g.Key, Count = g.Count() })
            .ToList();

        var grouped2 = range2Logs.GroupBy(l => l.AlarmMessage)
            .Select(g => new { AlarmMessage = g.Key, Count = g.Count() })
            .ToList();

        var merged = grouped1
            .Union(grouped2)
            .GroupBy(x => x.AlarmMessage)
            .Select(g => new ComparisonResult
            {
                AlarmMessage = g.Key,
                CountWeek1 = grouped1.FirstOrDefault(x => x.AlarmMessage == g.Key)?.Count ?? 0,
                CountWeek2 = grouped2.FirstOrDefault(x => x.AlarmMessage == g.Key)?.Count ?? 0
            })
            .ToList();

        return Ok(merged);
    }
    catch (Exception ex)
    {
        return BadRequest($"Dates range comparison error: {ex.Message}");
    }
}
    
    [HttpGet("min-max-generationtime")]
    public async Task<ActionResult<MinMaxGenerationTimeDto>> GetMinMaxGenerationTime()
    {
        await using var context = new LogAnalizerServerDbContext(new DbContextOptionsBuilder<LogAnalizerServerDbContext>()
            .UseSqlServer(DatabaseConnectionManager.CurrentConnectionString)
            .Options);

        if (!await context.AlarmLogs.AnyAsync())
        {
            return NotFound("No logs in DB.");
        }

        var minDate = await context.AlarmLogs.MinAsync(l => l.GenerationTime);
        var maxDate = await context.AlarmLogs.MaxAsync(l => l.GenerationTime);

        return Ok(new MinMaxGenerationTimeDto
        {
            Min = minDate,
            Max = maxDate
        });
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteLogForWeek([FromQuery] LogWeekType weekType)
    {
        var deleted = await _logService.DeleteLogsByWeekTypeAsync(weekType);
        return deleted ? Ok("Log deleted") : NotFound("No log found for this week");
    }
    
    
    
}