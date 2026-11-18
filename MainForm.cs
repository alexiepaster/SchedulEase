using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchedulEaseApp
{
    public partial class MainForm : Form
    {
        private Label lblTitle;
        private Button btnTeacher;
        private Button btnStudent;

        public MainForm()
        {
           
            SetupCustomUI();
        }

        private void SetupCustomUI()
        {
            // Form Settings
            this.Text = "SchedulEase - Welcome";
            this.Size = new Size(450, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 18, 18); // Dark background

            // System Name Label (Bold Maroon Font)
            lblTitle = new Label();
            lblTitle.Text = "SchedulEase";
            lblTitle.Font = new Font("Segoe UI", 32, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(128, 0, 32); // Deep Maroon
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Size = new Size(400, 80);
            lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, 70);
            this.Controls.Add(lblTitle);

            // Teacher Button ("T")
            btnTeacher = CreateCardButton("Teacher", 190);
            btnTeacher.Click += (s, e) => {
                MessageBox.Show("Navigating to Teacher Login...", "SchedulEase");
                // Open Teacher Login Form here
            };
            this.Controls.Add(btnTeacher);

            // Student Button ("S")
            btnStudent = CreateCardButton("Student", 290);
            btnStudent.Click += (s, e) => {
                MessageBox.Show("Navigating to Student Login...", "SchedulEase");
                // Open Student Login Form here
            };
            this.Controls.Add(btnStudent);
        }

        private Button CreateCardButton(string text, int topPosition)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(128, 0, 32); // Maroon button
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(280, 65);
            btn.Location = new Point((this.ClientSize.Width - btn.Width) / 2, topPosition);
            btn.Cursor = Cursors.Hand;

            // Custom Paint for soft card shadow & rounded feel
            btn.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw a subtle dark shadow underneath the button
                using (Brush shadowBrush = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
                {
                    g.FillRectangle(shadowBrush, 4, 6, btn.Width - 4, btn.Height - 4);
                }
            };

            return btn;
        }
    }
}