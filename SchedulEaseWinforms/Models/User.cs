namespace SchedulEaseWinforms.Models
{
    public abstract class User
    {
        public string IdNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public bool Login(string inputId, string inputPassword) =>
            !string.IsNullOrWhiteSpace(inputId) &&
            !string.IsNullOrWhiteSpace(inputPassword) &&
            string.Equals(IdNumber, inputId.Trim(), StringComparison.OrdinalIgnoreCase) &&
            Password == inputPassword;

        public void Logout() { }
        public abstract string DisplayDashboard();
    }
}
