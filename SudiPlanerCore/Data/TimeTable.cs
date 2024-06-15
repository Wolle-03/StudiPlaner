using static System.Reflection.Metadata.BlobBuilder;

namespace StudiPlaner.Core.Data;

public class TimeTable(List<RunningCourse> courses)
{
    public List<RunningCourse> Courses { get; set; } = courses;

    public int Count()
    {
        int counter = 0;
        foreach (RunningCourse course in Courses)
            counter += course.TimeSlots.Count;
        return counter;
    }

    public void Remove(int index)
    {
        int counter = 0;
        for (int i = 0; i < Courses.Count; i++)
            for (int j = 0; j < Courses[i].TimeSlots.Count; j++)
                if (counter++ == index)
                {
                    Courses[i].TimeSlots.RemoveAt(j);
                    if (Courses[i].TimeSlots.Count == 0)
                        Courses.RemoveAt(i);
                    return;
                }
    }
    public FinishedCourse Finish(int index,double grade)
    {
        FinishedCourse res = Courses[index].Finish(grade);
        Courses.RemoveAt(index);
        return res;
    }

    public override string ToString()
    {
        List<(TimeSlot, string)> slots = [];
        foreach (RunningCourse course in Courses)
            foreach (TimeSlot slot in course.TimeSlots)
                slots.Add((slot, course.Name));
        slots.Sort((x, y) => x.Item1.CompareTo(y.Item1));
        List<string> res = [];
        {
            int i = 0;
            slots.ForEach(x => res.Add($"ID {i++}: {x.Item1.Day} {x.Item1.Time.ToString()[1..].Replace('_', ':'),5} - {x.Item2}"));
        }
        return string.Join("\n", res);
    }

    public string PrintCourses()
    {
        List<string> courses = [];
        foreach (RunningCourse course in Courses)
            courses.Add(course.Name);
        courses.Sort((x, y) => x.CompareTo(y));
        List<string> res = [];
        {
            int i = 0;
            courses.ForEach(x => res.Add($"ID {i++}: {x}"));
        }
        return string.Join("\n", res);
    }
}
