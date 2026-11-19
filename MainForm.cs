using System;
using System.Drawing;
using System.Windows.Forms;

namespace SchedulEaseApp
{
    public partial class MainForm : Form
    {
        // 1. BACKEND DATA: Create one sample Teacher object to test with
        private Teacher sampleTeacher = new Teacher("T-101", "1234", "Dela Cruz");

        // 2. SCREEN CONTROLS: Declare all labels, buttons, and textboxes at the top
        private Label lblTitle;
        private Label lblSubtitle;

        private Button btnTeacher;
        private Button btnStudent;

        private TextBox txtId;
        private TextBox txtPass;
        private Button btnLogin;
        private Button btnBack;

        private Button btnLogout;

        // Constructor: Runs when the app starts
        public MainForm()
        {
            SetupWindow();
            CreateControls();
        }

        // Sets the size, title, and dark background of the window
        private void SetupWindow()
        {
            this.Text = "SchedulEase";
            this.Size = new Size(420, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 18, 18); // Dark gray/black
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        // Creates every label, button, and textbox and places them on the screen
        private void CreateControls()
        {
            // --- TITLE & SUBTITLE ---
            lblTitle = new Label();
            lblTitle.Text = "SchedulEase";
            lblTitle.Font = new Font("Segoe UI", 30, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(128, 0, 32); // Maroon
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Size = new Size(380, 60);
            lblTitle.Location = new Point(12, 45);
            this.Controls.Add(lblTitle);

            lblSubtitle = new Label();
            lblSubtitle.Text = "Select your role to continue";
            lblSubtitle.Font = new Font("Segoe UI", 11);
            lblSubtitle.ForeColor = Color.LightGray;
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitle.Size = new Size(380, 30);
            lblSubtitle.Location = new Point(12, 110);
            this.Controls.Add(lblSubtitle);

            // --- STEP 1 CONTROLS: ROLE SELECTION ---
            btnTeacher = CreateStyledButton("Teacher", 175, Color.FromArgb(128, 0, 32));
            btnTeacher.Click += BtnTeacher_Click; // Connects button click to method below
            this.Controls.Add(btnTeacher);

            btnStudent = CreateStyledButton("Student", 250, Color.FromArgb(45, 45, 45));
            btnStudent.Click += BtnStudent_Click;
            this.Controls.Add(btnStudent);

            // --- STEP 2 CONTROLS: LOGIN INPUTS (Hidden at start) ---
            txtId = CreateStyledTextBox("Teacher ID (T-101)", 170);
            this.Controls.Add(txtId);

            txtPass = CreateStyledTextBox("Password (1234)", 225);
            txtPass.UseSystemPasswordChar = true; // Hides password with dots
            this.Controls.Add(txtPass);

            btnLogin = CreateStyledButton("Login", 290, Color.FromArgb(128, 0, 32));
            btnLogin.Visible = false;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            btnBack = new Button();
            btnBack.Text = "← Back";
            btnBack.Font = new Font("Segoe UI", 10);
            btnBack.ForeColor = Color.LightGray;
            btnBack.BackColor = Color.Transparent;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Size = new Size(100, 35);
            btnBack.Location = new Point(152, 360);
            btnBack.Visible = false;
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);

            // --- STEP 3 CONTROLS: LOGOUT BUTTON (Hidden at start) ---
            btnLogout = CreateStyledButton("Logout", 250, Color.FromArgb(60, 60, 60));
            btnLogout.Visible = false;
            btnLogout.Click += BtnLogout_Click;
            this.Controls.Add(btnLogout);
        }

        // ================= BUTTON CLICK EVENTS =================

        // Runs when "Teacher" is clicked
        private void BtnTeacher_Click(object? sender, EventArgs e)
        {
            lblSubtitle.Text = "Teacher Portal Login";
            txtId.Clear();
            txtPass.Clear();

            // Hide Role buttons, Show Login boxes
            btnTeacher.Visible = false;
            btnStudent.Visible = false;

            txtId.Visible = true;
            txtPass.Visible = true;
            btnLogin.Visible = true;
            btnBack.Visible = true;
        }

        // Runs when "Student" is clicked
        private void BtnStudent_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Student module is coming soon!", "SchedulEase");
        }

        // Runs when "Login" is clicked
        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string typedId = txtId.Text.Trim();
            string typedPass = txtPass.Text;

            // Ask the Teacher object (using inherited User.Login) if credentials are right
            bool isSuccess = sampleTeacher.Login(typedId, typedPass);

            if (isSuccess == true)
            {
                // Show confirmation popup from Teacher.DisplayDashboard()
                sampleTeacher.DisplayDashboard();

                // Update screen to show Logout button
                lblSubtitle.Text = "Logged in as Teacher (T-101)";
                txtId.Visible = false;
                txtPass.Visible = false;
                btnLogin.Visible = false;
                btnBack.Visible = false;
                btnLogout.Visible = true;
            }
            else
            {
                MessageBox.Show("Wrong ID or Password! Use T-101 and 1234.", "Login Failed");
            }
        }

        // Runs when "← Back" is clicked
        private void BtnBack_Click(object? sender, EventArgs e)
        {
            ResetToHomeScreen();
        }

        // Runs when "Logout" is clicked
        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            // Calls inherited User.Logout()
            sampleTeacher.Logout();
            ResetToHomeScreen();
        }

        // Helper method to bring back the starting buttons
        private void ResetToHomeScreen()
        {
            lblSubtitle.Text = "Select your role to continue";

            btnTeacher.Visible = true;
            btnStudent.Visible = true;

            txtId.Visible = false;
            txtPass.Visible = false;
            btnLogin.Visible = false;
            btnBack.Visible = false;
            btnLogout.Visible = false;
        }

        // ================= DESIGN HELPERS =================
        // These two methods avoid repeating the same font/color code over and over

        private Button CreateStyledButton(string text, int topPosition, Color buttonColor)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.BackColor = buttonColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(270, 50);
            btn.Location = new Point(67, topPosition);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private TextBox CreateStyledTextBox(string placeholder, int topPosition)
        {
            TextBox box = new TextBox();
            box.PlaceholderText = placeholder;
            box.Font = new Font("Segoe UI", 12);
            box.BackColor = Color.FromArgb(35, 35, 35);
            box.ForeColor = Color.White;
            box.BorderStyle = BorderStyle.FixedSingle;
            box.Size = new Size(270, 35);
            box.Location = new Point(67, topPosition);
            box.Visible = false;
            return box;
        }
    }
}
