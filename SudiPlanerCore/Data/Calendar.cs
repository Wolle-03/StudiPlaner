namespace StudiPlaner.Core.Data;

public class Calendar : IForEachAble<Appointment>
{
    public List<Appointment> Appointments { get; set; } = [];

    public override string ToString()
    {
        Appointments.Sort((x, y) => x.Start.CompareTo(y.Start));
        string[] res = new string[Appointments.Count];
        int namelength = 0;
        Appointments.ForEach(x => namelength = namelength > x.Name.Length ? namelength : x.Name.Length);
        for (int i = 0; i < res.Length; i++)
            res[i] = Appointments[i].ToString(i, namelength);
        return string.Join("\n", res);
    }

    public void ForEach(IForEachAble<Appointment>.Del del)
    {
        foreach (Appointment appointment in Appointments)
        {
            del(appointment);
        }
    }
}
