namespace SchedulEase.Data;

/// <summary>
/// School accounts already exist, so there is NO sign-up.
/// Replace this list with your school's real account source (database / API) later.
/// </summary>
public static class AuthService
{
    const string DemoPassword = "SchedulEase2026";

    static readonly List<User> Accounts = new()
    {
        new Teacher { Id = "T-2026-041", Name = "Prof. Elena Reyes",      Password = DemoPassword },
        new Teacher { Id = "T-2026-052", Name = "Dr. Ramon Navarro",      Password = DemoPassword },
        new Teacher { Id = "T-2026-063", Name = "Prof. Liza Mendoza",     Password = DemoPassword },
        new Teacher { Id = "T-2026-074", Name = "Engr. Carlos Bautista",  Password = DemoPassword },
        new Teacher { Id = "T-2026-085", Name = "Prof. Maria Villanueva", Password = DemoPassword },
        new Teacher { Id = "T-2026-096", Name = "Dr. Jose Fernandez",     Password = DemoPassword },

        new Student { Id = "2025-10482", Name = "Marco Santos",   Password = DemoPassword },
        new Student { Id = "2025-10517", Name = "Angela Cruz",    Password = DemoPassword },
        new Student { Id = "2025-10603", Name = "John Dela Vega", Password = DemoPassword },
    };

    public static User Login(UserRole role, string id, string password)
        => Accounts.FirstOrDefault(u => u.Role == role
                                        && string.Equals(u.Id, (id ?? "").Trim(), StringComparison.OrdinalIgnoreCase)
                                        && u.Password == password);

    /// <summary>Shown under the login form so you can test quickly. Delete it for production.</summary>
    public static string DemoHint(UserRole role)
        => role == UserRole.Teacher ? $"Demo: T-2026-041 / {DemoPassword}" : $"Demo: 2025-10482 / {DemoPassword}";
}
