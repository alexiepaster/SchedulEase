namespace SchedulEase.Pages;

/// <summary>Login for the chosen role. There is no sign-up: school accounts already exist.</summary>
public class LoginPage : Panel
{
    public event Action Back;
    public event Action<User> LoggedIn;

    readonly UserRole _role;
    readonly FieldBox _id, _password;
    readonly TextLabel _error;

    const int SheetTop = 150;

    public LoginPage(UserRole role)
    {
        _role = role;
        DoubleBuffered = true;
        bool teacher = role == UserRole.Teacher;

        // --- Maroon top area (painted in OnPaintBackground) ---
        Ui.Btn(this, "←  Change Role", 20, 16, 124, 30, BtnLook.Glass, () => Back?.Invoke(), 15, 12);
        var tag = PillLabel.Create("SCHOOL ACCOUNT", Color.FromArgb(64, 0, 0, 0), Theme.Rose200, 10, 10);
        tag.Location = new Point(Theme.PageWidth - 20 - tag.Width, 23);
        Controls.Add(tag);
        Ui.Text(this, teacher ? "Teacher Portal" : "Student Portal", 20, 62, 340, 32, 24, Color.White, true);
        Ui.Text(this, $"Sign in with your school-issued {(teacher ? "teacher" : "student")} account", 20, 98, 350, 18, 12.5f, Theme.Rose100);

        // --- White-ish sheet ---
        var sheet = new RoundedPanel { Radius = 32, Fill = Theme.Canvas, Bounds = new Rectangle(0, SheetTop, Theme.PageWidth, 700) };
        Controls.Add(sheet);

        int y = 30;
        _id = Ui.LabeledField(sheet, teacher ? "Teacher ID" : "Student ID", "", 22, ref y, 346, teacher ? "e.g. T-2026-041" : "e.g. 2025-10482");

        Ui.Text(sheet, "Password", 22, y, 346, 16, 12, Theme.TextPrimary, true);
        _password = FieldBox.TextInput("Enter your password", true);
        Ui.Field(sheet, _password, 22, y + 22, 346);
        y += 70;

        _error = Ui.Text(sheet, "", 22, y, 346, 18, 12, Theme.Danger, true);
        y += 22;

        Ui.Btn(sheet, "Login", 22, y, 346, 46, BtnLook.Gradient, TryLogin, 16, 13.5f);
        y += 46 + 24;

        var line = new Panel { BackColor = Theme.BorderLight, Bounds = new Rectangle(22, y, 346, 1) };
        sheet.Controls.Add(line);
        Ui.Text(sheet, "Accounts are issued by your school. Forgot your password? Please contact the registrar or IT office.",
                22, y + 12, 346, 34, 12, Theme.TextSecondary, false, ContentAlignment.TopCenter);
        Ui.Text(sheet, AuthService.DemoHint(role), 22, y + 56, 346, 16, 11, Theme.TextMuted, false, ContentAlignment.TopCenter, true);

        _id.Box.KeyDown += OnEnterKey;
        _password.Box.KeyDown += OnEnterKey;
        VisibleChanged += (_, _) => { if (Visible) _id.Box.Focus(); };
    }

    void OnEnterKey(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        TryLogin();
    }

    void TryLogin()
    {
        var user = AuthService.Login(_role, _id.Box.Text, _password.Box.Text);
        if (user == null)
        {
            _error.Text = "Incorrect ID or password. Please try again.";
            return;
        }
        LoggedIn?.Invoke(user);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Theme.MaroonDark);
        if (Width < 2) return;
        using var br = new LinearGradientBrush(new Rectangle(0, 0, Width, SheetTop + 20), Theme.MaroonPrimary, Theme.MaroonDark, 90f);
        g.FillRectangle(br, 0, 0, Width, SheetTop + 20);
    }
}
