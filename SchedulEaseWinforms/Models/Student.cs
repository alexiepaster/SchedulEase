namespace SchedulEaseWinforms.Models
{
    public class Student : User
    {
        public string StudentID { get => IdNumber; set => IdNumber = value; }
        public string StudentName { get => Name; set => Name = value; }

        public override string DisplayDashboard() => $"Student Portal — {StudentName}";

        public ClassSession? SearchClassCode(IEnumerable<ClassSession> activeSessions, string code)
        {
            var value = (code ?? string.Empty).Trim();
            if (value.Length == 0) return null;
            return activeSessions.FirstOrDefault(s =>
                s.ClassCode.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                s.SubjectCode.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                s.Section.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                s.AssignedRoom?.RoomNumber.Contains(value, StringComparison.OrdinalIgnoreCase) == true);
        }
    }
}
