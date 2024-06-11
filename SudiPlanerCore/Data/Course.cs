namespace StudiPlaner.Core.Data;

public class Course(string name, int semester)
{
    public string Name { get; set; } = name;
    public int Semester { get; set; } = semester;
}
public class RunningCourse(string name, int semester, params TimeSlot[] timeSlot) : Course(name, semester)
{
    public TimeSlot[] TimeSlot { get; set; } = timeSlot;
}

public class FinishedCourse(string name, int semester, double grade) : Course(name, semester)
{
    public double Grade { get; set; } = grade;
}
