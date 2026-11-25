namespace SchedulEase.Pages;

/// <summary>First screen: "Who is signing in today?" Teacher or Student.</summary>
public class RoleSelectPage : GradientPanel
{
    public event Action<UserRole> RoleChosen;

    readonly TextLabel _title, _subtitle, _prompt, _note;
    readonly RoleCard _teacher, _student;
    int _logoY = 60;

    public RoleSelectPage()
    {
        Colors = new[] { Theme.MaroonPrimary, Theme.MaroonDark, Theme.MaroonDeep };
        Positions = new[] { 0f, 0.52f, 1f };
        Angle = 80f;

        _title = Ui.Text(this, "SchedulEase", 0, 0, Theme.PageWidth, 40, 32, Color.White, true, ContentAlignment.TopCenter);
        _subtitle = Ui.Text(this, "Real-Time Classroom Availability and Room Locator System", 62, 0, 266, 40, 13, Theme.Rose100, false, ContentAlignment.TopCenter);

        _prompt = Ui.Text(this, "WHO IS SIGNING IN TODAY?", 0, 0, Theme.PageWidth, 18, 11.5f, Theme.Rose200, true, ContentAlignment.TopCenter);
        _teacher = new RoleCard(UserRole.Teacher) { Size = new Size(342, 88), Left = 24 };
        _student = new RoleCard(UserRole.Student) { Size = new Size(342, 88), Left = 24 };
        _teacher.Click += (_, _) => RoleChosen?.Invoke(UserRole.Teacher);
        _student.Click += (_, _) => RoleChosen?.Invoke(UserRole.Student);
        Controls.Add(_teacher);
        Controls.Add(_student);
        _note = Ui.Text(this, "Sign in with your school-issued account", 0, 0, Theme.PageWidth, 16, 11, Color.FromArgb(231, 180, 186), false, ContentAlignment.TopCenter);

        DoLayout();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        DoLayout();
    }

    // Stack the role buttons from the bottom so the page adapts to the window height.
    void DoLayout()
    {
        if (_note == null) return;
        _note.Top = Height - 24 - 16;
        _student.Top = _note.Top - 6 - 88;
        _teacher.Top = _student.Top - 6 - 88;
        _prompt.Top = _teacher.Top - 10 - 18;

        _logoY = Math.Max(20, (_prompt.Top - 190) / 2);
        _title.Top = _logoY + 82 + 18;
        _subtitle.Top = _title.Top + 44;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Soft background orbs
        using (var path = new GraphicsPath())
        {
            path.AddEllipse(Width - 190, -70, 260, 260);
            using var pg = new PathGradientBrush(path)
            {
                CenterColor = Color.FromArgb(40, 255, 255, 255),
                SurroundColors = new[] { Color.FromArgb(0, 255, 255, 255) }
            };
            g.FillPath(pg, path);
        }
        using (var pen = new Pen(Color.FromArgb(18, 255, 255, 255), 1.5f))
            g.DrawEllipse(pen, -80, Height - 330, 230, 230);

        // Logo box
        int x = (Width - 82) / 2;
        for (int i = 1; i <= 6; i++)
            Gfx.FillRound(g, Color.FromArgb(10, 0, 0, 0), new RectangleF(x, _logoY + i * 2, 82, 82), 26);
        Gfx.FillRound(g, Color.White, new RectangleF(x, _logoY, 82, 82), 26);
        Icons.Draw(g, Glyph.Calendar, new RectangleF(x + 19, _logoY + 19, 44, 44), Theme.MaroonPrimary, 2.2f);
    }
}

/// <summary>The big Teacher / Student button (teacher = solid white, student = glass).</summary>
public class RoleCard : Control
{
    readonly UserRole _role;
    bool _hover;

    public RoleCard(UserRole role)
    {
        _role = role;
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        BackColor = Color.Transparent;
        Cursor = Cursors.Hand;
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        bool teacher = _role == UserRole.Teacher;
        var r = new RectangleF(0.5f, 0.5f, Width - 1, 79);

        if (teacher)
        {
            for (int i = 1; i <= 6; i++)
                Gfx.FillRound(g, Color.FromArgb(9, 0, 0, 0), new RectangleF(r.X, r.Y + i, r.Width, r.Height), 24);
            Gfx.FillRound(g, _hover ? Color.FromArgb(255, 241, 242) : Color.White, r, 24);
        }
        else
        {
            Gfx.FillRound(g, Color.FromArgb(_hover ? 51 : 31, 255, 255, 255), r, 24);
            Gfx.DrawRound(g, Color.FromArgb(71, 255, 255, 255), r, 24, 1.5f);
        }

        // Icon tile
        var tile = new RectangleF(18, 16, 48, 48);
        Gfx.FillRound(g, teacher ? Theme.MaroonPrimary : Color.White, tile, 16);
        Icons.Draw(g, teacher ? Glyph.Person : Glyph.Cap, new RectangleF(30, 28, 24, 24), teacher ? Color.White : Theme.MaroonPrimary, 2.2f);

        // Texts
        Gfx.Text(g, teacher ? "I am a Teacher" : "I am a Student", Theme.F(16, true), new Rectangle(80, 18, 200, 22), teacher ? Theme.TextPrimary : Color.White);
        Gfx.Text(g, teacher ? "Manage rooms & live class sessions" : "Find classrooms & check live status",
                 Theme.F(11.5f), new Rectangle(80, 42, 215, 18), teacher ? Theme.TextSecondary : Theme.Rose100);

        // Arrow circle
        var arrow = new RectangleF(Width - 18 - 32, 24, 32, 32);
        Gfx.FillRound(g, teacher ? Theme.MaroonSoft : Color.FromArgb(46, 255, 255, 255), arrow, 16);
        Icons.Draw(g, Glyph.Arrow, new RectangleF(arrow.X + 8, arrow.Y + 8, 16, 16), teacher ? Theme.MaroonPrimary : Color.White, 2.4f);
    }
}
