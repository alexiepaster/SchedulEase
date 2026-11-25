namespace SchedulEase.Models;

public enum UserRole { Teacher, Student }

public abstract class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public abstract UserRole Role { get; }

    // Name words without titles like "Prof." / "Dr."
    string[] Words => Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(w => !w.EndsWith(".")).ToArray();

    public string FirstName => Words.FirstOrDefault() ?? Name;

    public string Initials
    {
        get
        {
            var w = Words;
            if (w.Length == 0) return "?";
            if (w.Length == 1) return w[0].Substring(0, 1).ToUpper();
            return (w[0].Substring(0, 1) + w[^1].Substring(0, 1)).ToUpper();
        }
    }
}

public class Teacher : User { public override UserRole Role => UserRole.Teacher; }
public class Student : User { public override UserRole Role => UserRole.Student; }
