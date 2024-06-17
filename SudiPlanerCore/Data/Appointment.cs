namespace StudiPlaner.Core.Data;

public class Appointment(string name, DateTime start, string? description = null)
{
    public string Name { get; set; } = name;
    public DateTime Start { get; set; } = start;
    public string? Description { get; set; } = description;

    public string ToString(int id, int nameLength)
    {
        string padding = "";
        for (int i = Name.Length; i < nameLength; i++)
            padding += " ";
        return $"ID {id}: {padding}{Name} - {Start}{(Description != null && Description != "" ? $" - {Description}" : "")}";
    }
}