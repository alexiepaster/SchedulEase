using System.Windows.Forms;

namespace SchedulEaseApp
{
    // ": User" means Teacher is a child of User (Inheritance).
    public class Teacher : User
    {
        // Extra attributes specific to a Teacher (from your UML)
        private string teacherID;
        private string teacherName;

        // Constructor: Takes the info and sends it up to the parent User class using ": base(...)"
        public Teacher(string idNumber, string password, string name) : base(idNumber, password, name)
        {
            this.teacherID = idNumber;
            this.teacherName = name;
        }

        // "override" fulfills the promise to write Teacher's own version of DisplayDashboard() (Polymorphism).
        public override void DisplayDashboard()
        {
            string message = "Login Successful!\n\n" +
                             "Welcome, Prof. " + teacherName + "\n" +
                             "Teacher ID: " + teacherID;

            MessageBox.Show(message, "SchedulEase - Teacher Confirmed");
        }

        // Other UML methods for Teacher (Empty for now until you add room features later)
        public void EnterClassCode() { }
        public void ViewAvailableRooms() { }
        public void SelectRoom() { }
        public void OccupyRoom() { }
        public void ClockOut() { }
    }
}
