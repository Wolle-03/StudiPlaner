namespace StudiPlaner.Core.Data;

public class TimeTable(params RunningCourse[] courses)
{
    public RunningCourse[] Courses { get; set; } = courses;
}
