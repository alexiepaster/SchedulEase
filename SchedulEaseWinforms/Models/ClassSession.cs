namespace SchedulEaseWinforms.Models
{
    public class ClassSession
    {
        public string SessionID { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpectedEndTime { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public Classroom? AssignedRoom { get; set; }

        public DateTime CalculateEndTime() => StartTime.AddMinutes(DurationMinutes);
        public bool CheckSessionStatus() => DateTime.Now < ExpectedEndTime;
        public string GetSchedule() => $"{SubjectCode} ({ClassCode}) • Sec {Section}";
        public TimeSpan RemainingTime => ExpectedEndTime - DateTime.Now;
    }
}
