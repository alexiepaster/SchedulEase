namespace SchedulEase.Models;

public class ClassSession
{
    public string ClassCode { get; set; }
    public string Subject { get; set; }
    public string Section { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime StartTime { get; set; }
    public string RoomId { get; set; }
    public string TeacherId { get; set; }
    public string TeacherName { get; set; }

    public DateTime EndTime => StartTime.AddMinutes(DurationMinutes);

    public TimeSpan Remaining
    {
        get
        {
            var left = EndTime - DateTime.Now;
            return left < TimeSpan.Zero ? TimeSpan.Zero : left;
        }
    }
}
