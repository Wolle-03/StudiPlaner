using StudiPlaner.Core.Data;

namespace Core.Data;

[Serializable]
public class TimeSlotBlockedException(TimeSlot timeSlot) : Exception
{
    public override string ToString() => $"The timeslot -{(string)timeSlot}- is already taken";
}