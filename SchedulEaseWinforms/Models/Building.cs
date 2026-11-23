namespace SchedulEaseWinforms.Models
{
    public class Building
    {
        public string BuildingID { get; set; } = string.Empty;
        public string BuildingName { get; set; } = string.Empty;
        public List<Classroom> Rooms { get; set; } = new();

        public List<Classroom> GetBuildingRooms() => Rooms;
        public string ViewBuildingDetails() => $"{BuildingName} ({Rooms.Count} Rooms)";
    }
}
