namespace LogAnalizerShared;

public class ComparisonResult
{
    public int Id { get; set; }

    public string AlarmMessage { get; set; } = null!;

    public int CountWeek1 { get; set; }
    public int CountWeek2 { get; set; }
    
    public string FinalState { get; set; }
   

    public LogWeekType Week1Type { get; set; }
    public LogWeekType Week2Type { get; set; }
}