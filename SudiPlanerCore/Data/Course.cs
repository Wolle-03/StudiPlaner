namespace StudiPlaner.Core.Data;

public abstract class Course(string name, int semester)
{
    public string Name { get; set; } = name;
    public int Semester { get; set; } = semester;
}
public class RunningCourse(string name, int semester, List<TimeSlot>? timeSlots = null) : Course(name, semester), IForEachAble<TimeSlot>
{
    public List<TimeSlot> TimeSlots { get; set; } = timeSlots ?? [];

    public FinishedCourse Finish(double grade) => new(Name, Semester, grade);

    public void ForEach(IForEachAble<TimeSlot>.Del del)
    {
        foreach (TimeSlot slot in TimeSlots)
        {
            del(slot);
        }
    }
}

public class FinishedCourse(string name, int semester, double grade) : Course(name, semester)
{
    public double Grade { get; set; } = grade;
}
