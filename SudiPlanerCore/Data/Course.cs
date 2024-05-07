namespace StudiPlaner.Core.Data;

public struct Course(string name, int semester, double? grade, params TimeSlot[] timeSlot)
{
    public string Name { get; set; } = name;
    public int Semester { get; set; } = semester;
    public double? Grade { get; set; } = grade;
    public TimeSlot[] TimeSlot { get; set; } = timeSlot;
}
