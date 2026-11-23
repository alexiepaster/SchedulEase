namespace SchedulEaseWinforms.Models
{
    public class Classroom
    {
        public string RoomID { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string Status { get; set; } = "Available";
        public string BuildingName { get; set; } = string.Empty;
        public int Capacity { get; set; } = 40;
        public string Equipment { get; set; } = "Projector, whiteboard";
        public ClassSession? ActiveSession { get; set; }

        public bool CheckAvailability() =>
            Status.Equals("Available", StringComparison.OrdinalIgnoreCase);

        public string GetRoomDetails() =>
            CheckAvailability()
                ? $"{RoomNumber} ({BuildingName}) is currently AVAILABLE for occupancy."
                : $"{RoomNumber} ({BuildingName}) is OCCUPIED by {ActiveSession?.TeacherName} ({ActiveSession?.SubjectCode} - {ActiveSession?.ClassCode}).";

        public void UpdateStatus(string newStatus, ClassSession? session)
        {
            Status = newStatus;
            ActiveSession = session;
        }
    }
}
