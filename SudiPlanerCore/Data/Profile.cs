namespace StudiPlaner.Core.Data;

public class Profile(string name)
{
    public string Name { get; set; } = name;
    public Calendar Calendar { get; set; } = new();
    public TimeTable TimeTable { get; set; } = new([]);
    public Grades Grades { get; set; } = new();
}