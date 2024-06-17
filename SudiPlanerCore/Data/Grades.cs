namespace StudiPlaner.Core.Data;

public class Grades : IForEachAble<FinishedCourse>
{
    public List<FinishedCourse> FinishedCourses { get; set; } = [];

    public override string ToString()
    {
        if (FinishedCourses.Count == 0)
            return "";
        List<string> res = [];
        FinishedCourses.Sort((x, y) => x.Name.CompareTo(y.Name));
        FinishedCourses.Sort((x, y) => x.Semester.CompareTo(y.Semester));
        int semester = FinishedCourses[0].Semester;
        List<double> grades = [];
        List<double> semesterGrades = [];
        double semesterGrade = 0;
        foreach (FinishedCourse course in FinishedCourses)
        {
            if (course.Semester != semester)
            {
                semesterGrade = 0;
                semesterGrades.ForEach(x => semesterGrade += x);
                semesterGrade /= semesterGrades.Count;
                res.Add($"{semester++}. Semester: {semesterGrade:f1}\n");
                semesterGrades.Clear();
            }
            grades.Add(course.Grade);
            semesterGrades.Add(course.Grade);
            res.Add($"{semester}. Semester - {course.Name} - {course.Grade:f1}");
        }
        semesterGrade = 0;
        semesterGrades.ForEach(x => semesterGrade += x);
        semesterGrade /= semesterGrades.Count;
        res.Add($"{semester++}. Semester: {semesterGrade:f2}\n");
        double grade = 0;
        grades.ForEach(x => grade += x);
        grade /= grades.Count;
        res.Add($"All: {grade:f2}");
        return string.Join("\n", res);
    }

    public string ToStringByIDs()
    {
        List<string> res = [];
        FinishedCourses.Sort((x, y) => x.Name.CompareTo(y.Name));
        FinishedCourses.Sort((x, y) => x.Semester.CompareTo(y.Semester));
        {
            int id = 0;
            FinishedCourses.ForEach(x => res.Add($"ID {id++} - {x.Semester}. Semester - {x.Name} - {x.Grade:f1}"));
        }
        return string.Join("\n", res);
    }

   public void ForEach(IForEachAble<FinishedCourse>.Del del)
    {
        foreach (FinishedCourse course in FinishedCourses)
        {
            del(course);
        }
    }
}