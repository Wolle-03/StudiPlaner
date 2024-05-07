namespace StudiPlaner.Core.Data;

public struct TimeSlot(Day Day, Time Time)
{
    public Day Day { get; set; } = Day;
    public Time Time { get; set; } = Time;
}

public enum Day { Mon, Tue, Wed, Thu, Fri, Sat, Sun }

public enum Time { _8_00, _9_45, _11_30, _14_00, _15_45, _17_30, _19_15 }