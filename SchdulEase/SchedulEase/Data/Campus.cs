namespace SchedulEase.Data;

/// <summary>In-memory buildings, classrooms and class sessions (with sample data).</summary>
public static class Campus
{
    public static readonly List<Building> Buildings = new()
    {
        new Building { Id = 1, Name = "Main Building" },
        new Building { Id = 2, Name = "Science & Tech Wing" },
        new Building { Id = 3, Name = "CS Lab Annex" },
    };

    public static readonly List<Classroom> Rooms = new();
    public static readonly List<ClassSession> Sessions = new();

    static Campus() => Seed();

    // ---------- Queries ----------
    public static Building FindBuilding(int id) => Buildings.First(b => b.Id == id);
    public static Classroom FindRoom(string id) => Rooms.First(r => r.Id == id);
    public static ClassSession SessionOfRoom(string roomId) => Sessions.FirstOrDefault(s => s.RoomId == roomId);
    public static ClassSession SessionOfTeacher(string teacherId) => Sessions.FirstOrDefault(s => s.TeacherId == teacherId);

    /// <summary>buildingId = 0 means the whole campus.</summary>
    public static (int Total, int Available, int Occupied, int Reserved) Stats(int buildingId = 0)
    {
        var rooms = Rooms.Where(r => buildingId == 0 || r.BuildingId == buildingId).ToList();
        return (rooms.Count,
                rooms.Count(r => r.Status == RoomStatus.Available),
                rooms.Count(r => r.Status == RoomStatus.Occupied),
                rooms.Count(r => r.Status == RoomStatus.Reserved));
    }

    // ---------- Commands (return an error message, or null when OK) ----------
    public static string UpdateRoom(string roomId, string name, int capacity, string equipment, RoomStatus status)
    {
        var room = FindRoom(roomId);
        if (room.Status == RoomStatus.Occupied) return "This room is occupied, so it can't be edited.";
        if (status == RoomStatus.Occupied) return "Use Class Session to occupy a room.";
        if (string.IsNullOrWhiteSpace(name)) return "Room name is required.";
        if (capacity < 1 || capacity > 500) return "Capacity must be between 1 and 500.";
        if (Rooms.Any(r => r.Id != room.Id && r.BuildingId == room.BuildingId
                           && string.Equals(r.Name, name.Trim(), StringComparison.OrdinalIgnoreCase)))
            return "Another room in this building already uses that name.";

        room.Name = name.Trim();
        room.Capacity = capacity;
        room.Equipment = string.IsNullOrWhiteSpace(equipment) ? "None" : equipment.Trim();
        room.Status = status;
        return null;
    }

    public static string StartSession(User teacher, string code, string subject, string section, int minutes, string roomId)
    {
        if (SessionOfTeacher(teacher.Id) != null) return "You already have an active session.";
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(section))
            return "Please fill in class code, subject and section.";
        var room = FindRoom(roomId);
        if (room.Status != RoomStatus.Available) return "That room is no longer available.";

        room.Status = RoomStatus.Occupied;
        Sessions.Add(new ClassSession
        {
            ClassCode = code.Trim(), Subject = subject.Trim(), Section = section.Trim(),
            DurationMinutes = minutes, StartTime = DateTime.Now, RoomId = roomId,
            TeacherId = teacher.Id, TeacherName = teacher.Name
        });
        return null;
    }

    public static void EndSession(string teacherId)
    {
        var s = SessionOfTeacher(teacherId);
        if (s == null) return;
        Sessions.Remove(s);
        FindRoom(s.RoomId).Status = RoomStatus.Available;
    }

    // ---------- Sample data ----------
    static void Seed()
    {
        AddRooms(1, Enumerable.Range(101, 10).Select(n => $"Room {n}"), 45,
            new[] { "Smart TV, Whiteboard, AC", "Dual Projector, AC", "Projector, Whiteboard", "Smart Board, AC" });
        AddRooms(2, Enumerable.Range(201, 10).Select(n => $"Room {n}"), 38,
            new[] { "Lab Benches, Projector", "Chemistry Hoods, Smart Board", "Projector, AC", "Microscopes, Whiteboard" });
        AddRooms(3, new[] { "Lab A1", "Lab A2", "Lab A3", "Lab A4", "Lab A5", "Lab B1", "Lab B2", "Lab B3", "Lab B4", "Lab B5" }, 32,
            new[] { "36 Workstations, Fiber LAN", "Mac Studio Lab, 4K Display", "30 PCs, Projector", "Network Lab, Switches" });

        Occupy(1, "Room 103", "CS-201A", "Data Structures", "BSCS-2A", 90, 25, "T-2026-052", "Dr. Ramon Navarro");
        Occupy(1, "Room 107", "ENG-101", "Purposive Communication", "BSIT-1C", 60, 15, "T-2026-063", "Prof. Liza Mendoza");
        Occupy(2, "Room 202", "CHEM-102", "Organic Chemistry Lab", "BSBIO-2B", 120, 40, "T-2026-074", "Engr. Carlos Bautista");
        Occupy(2, "Room 205", "BIO-110", "General Biology", "BSBIO-1A", 90, 10, "T-2026-085", "Prof. Maria Villanueva");
        Occupy(3, "Lab A2", "IT-305", "Web Systems", "BSIT-3B", 120, 30, "T-2026-096", "Dr. Jose Fernandez");

        Reserve(1, "Room 109");
        Reserve(2, "Room 207");
        Reserve(3, "Lab B4");
        Reserve(3, "Lab B5");
    }

    static void AddRooms(int buildingId, IEnumerable<string> names, int baseCapacity, string[] equipment)
    {
        int i = 0;
        foreach (var name in names)
        {
            Rooms.Add(new Classroom
            {
                Id = $"B{buildingId}-{name.Replace(' ', '-')}",
                Name = name,
                BuildingId = buildingId,
                Status = RoomStatus.Available,
                Capacity = baseCapacity + (i % 3) * 5,
                Equipment = equipment[i % equipment.Length]
            });
            i++;
        }
    }

    static Classroom Find(int buildingId, string name) => Rooms.First(r => r.BuildingId == buildingId && r.Name == name);

    static void Reserve(int buildingId, string name) => Find(buildingId, name).Status = RoomStatus.Reserved;

    static void Occupy(int buildingId, string name, string code, string subject, string section,
                       int minutes, int minutesAgo, string teacherId, string teacherName)
    {
        var room = Find(buildingId, name);
        room.Status = RoomStatus.Occupied;
        Sessions.Add(new ClassSession
        {
            ClassCode = code, Subject = subject, Section = section, DurationMinutes = minutes,
            StartTime = DateTime.Now.AddMinutes(-minutesAgo), RoomId = room.Id,
            TeacherId = teacherId, TeacherName = teacherName
        });
    }
}
