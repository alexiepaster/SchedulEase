using System.Windows.Forms;

namespace SchedulEaseApp
{
    // "abstract" means this is a parent blueprint class.
    public abstract class User
    {
        // "protected" means ONLY this class and its child classes (like Teacher) can use these variables.
        protected string idNumber;
        protected string password;
        protected string name;

        // Constructor: Runs automatically when a new User/Teacher is created to set their details.
        public User(string idNumber, string password, string name)
        {
            this.idNumber = idNumber;
            this.password = password;
            this.name = name;
        }

        // UML Method 1: Checks if the typed ID and password match the saved ones.
        public bool Login(string inputId, string inputPassword)
        {
            if (this.idNumber == inputId && this.password == inputPassword)
            {
                return true;  // Login succeeded!
            }
            else
            {
                return false; // Login failed!
            }
        }

        // UML Method 2: Shows a goodbye message when logging out.
        public void Logout()
        {
            MessageBox.Show(name + " has logged out.", "SchedulEase");
        }

        // UML Method 3: "abstract" means this method has NO body here.
        // Every child class (Teacher, Student) is FORCED to write their own version of this.
        public abstract void DisplayDashboard();
    }
}
