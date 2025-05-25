using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogAnalizerServer.Models;


public enum LogWeekType
{
    Week1 = 1,  Week2 = 2,
    Week3 = 3, Week4 = 4, 
    Week5 = 5, Week6 = 6,
    Week7 = 7, Week8 = 8,
    Week9 = 9, Week10 = 10,
    Week11 = 11, Week12 = 12,
    Week13 = 13, Week14 = 14,
    Week15 = 15, Week16 = 16,
    Week17 = 17, Week18 = 18,
    Week19 = 19, Week20 = 20,
    Week21 = 21, Week22 = 22,
    Week23 = 23, Week24 = 24,
        
    Week25 = 25, Week26 = 26,
    Week27 = 27, Week28 = 28,
    Week29 = 29, Week30 = 30,
    Week31 = 31, Week32 = 32,
    Week33 = 33, Week34 = 34,
    Week35 = 35, Week36 = 36,
    Week37 = 37, Week38 = 38,
    Week39 = 39, Week40 = 40,
    Week41 = 41, Week42 = 42,
    Week43 = 43, Week44 = 44,
    Week45 = 45, Week46 = 46,
    Week47 = 47, Week48 = 48,
    Week49 = 49, Week50 = 50,
    Week51 = 51, Week52 = 52,
    Week53 = 53
}

    public class AlarmLog
    {
        
        public int Id { get; set; } 

        public DateTime TimeWhenLogged { get; set; }
        public DateTime LocalZoneTime { get; set; }

        public long SequenceNumber { get; set; }
        public string AlarmId { get; set; } 
        public string AlarmClass { get; set; } = null!;
        public string Resource { get; set; } = null!;
        public string LoggedBy { get; set; } = null!;
        public string Reference { get; set; } = null!;

        public string PrevState { get; set; } = null!;
        public string LogAction { get; set; } = null!;
        public string FinalState { get; set; } = null!;

        public string AlarmMessage { get; set; } = null!;
        public DateTime GenerationTime { get; set; } 
        public DateTime GenerationTimeUtc { get; set; }

        public string Project { get; set; } = null!;
        public LogWeekType WeekType { get; set; } 
    }
