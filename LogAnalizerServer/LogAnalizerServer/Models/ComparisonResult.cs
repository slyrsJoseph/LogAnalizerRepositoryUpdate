
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogAnalizerServer.Models;

public class ComparisonResult
{
    [Key]
    public int Id { get; set; }

    public string AlarmMessage { get; set; } = null!;

    public int CountWeek1 { get; set; }
    public int CountWeek2 { get; set; }

    public LogWeekType Week1Type { get; set; }
    public LogWeekType Week2Type { get; set; }
    
    [NotMapped]
    public int Difference => CountWeek2 - CountWeek1;
    
    
}