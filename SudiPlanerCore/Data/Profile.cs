namespace StudiPlaner.Core.Data;

public readonly struct Profile
{
    public string Name { get; init; }
    public Calendar Calendar { get; init; }
    public TimeTable TimeTable { get; init; }
    public List<FinishedCourse> FinishedCourses { get; init; }
}