namespace StudiPlaner.Core.Data;

public struct TimeSlot(Day Day, Time Time) : IComparable<TimeSlot>
{
    public Day Day { get; set; } = Day;
    public Time Time { get; set; } = Time;

    public int CompareTo(TimeSlot other) => Day.CompareTo(other.Day) * 10 + Time.CompareTo(other.Time);

    public static explicit operator string(TimeSlot t) => $"{t.Day} {t.Time.ToString()[1..].Replace('_', ':'),5}";
}

public enum Day { Mon = 1, Tue, Wed, Thu, Fri, Sat, Sun }

public enum Time { _8_00 = 1, _9_45, _11_30, _14_00, _15_45, _17_30, _19_15 }