namespace SchedulEase.Models;

public enum RoomStatus { Available, Occupied, Reserved }

public class Classroom
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int BuildingId { get; set; }
    public RoomStatus Status { get; set; }
    public int Capacity { get; set; }
    public string Equipment { get; set; }
}
