namespace SchedulEaseWinforms.Models
{
    public class Teacher : User
    {
        public string TeacherID { get => IdNumber; set => IdNumber = value; }
        public string TeacherName { get => Name; set => Name = value; }

        public override string DisplayDashboard() => $"Teacher Portal — {TeacherName}";

        public ClassSession OccupyRoom(Classroom room, string classCode, string subjectCode,
            string section, int durationMinutes)
        {
            if (!room.CheckAvailability())
                throw new InvalidOperationException("This classroom is no longer available.");

            var now = DateTime.Now;
            var session = new ClassSession
            {
                SessionID = "SES-" + Guid.NewGuid().ToString("N")[..5].ToUpperInvariant(),
                ClassCode = classCode.Trim(),
                SubjectCode = subjectCode.Trim(),
                Section = section.Trim(),
                DurationMinutes = durationMinutes,
                StartTime = now,
                ExpectedEndTime = now.AddMinutes(durationMinutes),
                TeacherName = TeacherName,
                AssignedRoom = room
            };
            room.UpdateStatus("Occupied", session);
            return session;
        }

        public void ClockOut(Classroom room) => room.UpdateStatus("Available", null);
    }
}
