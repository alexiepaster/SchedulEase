namespace SchedulEase.Shell;

/// <summary>Maroon portal header: hamburger button + avatar, role pill, ID and name.</summary>
public class HeaderBar : GradientPanel
{
    static readonly Rectangle MenuRect = new Rectangle(18, 18, 40, 40);
    readonly User _user;
    public event Action MenuClicked;

    public HeaderBar(User user)
    {
        _user = user;
        Colors = new[] { Theme.MaroonPrimary, Theme.MaroonDark };
        Angle = 90f;
        Height = 76;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Hamburger button
        Gfx.FillRound(g, Color.FromArgb(36, 255, 255, 255), MenuRect, 14);
        Gfx.DrawRound(g, Color.FromArgb(51, 255, 255, 255), new RectangleF(18.5f, 18.5f, 39, 39), 14);
        Icons.Draw(g, Glyph.Menu, new RectangleF(28, 28, 20, 20), Color.White, 2.2f);

        // Avatar
        var avatar = new Rectangle(68, 18, 40, 40);
        Gfx.FillRound(g, Color.FromArgb(46, 255, 255, 255), avatar, 14);
        Gfx.DrawRound(g, Color.FromArgb(71, 255, 255, 255), new RectangleF(68.5f, 18.5f, 39, 39), 14);
        Gfx.Text(g, _user.Initials, Theme.F(14, true), avatar, Color.White, Gfx.Center);

        // Role pill + id
        string role = _user.Role.ToString().ToUpper();
        int pw = TextRenderer.MeasureText(role, Theme.F(9.5f, true), new Size(500, 50), TextFormatFlags.NoPadding).Width + 16;
        var pill = new Rectangle(120, 19, pw, 16);
        Gfx.FillRound(g, Color.FromArgb(41, 255, 255, 255), pill, 8);
        Gfx.Text(g, role, Theme.F(9.5f, true), pill, Theme.Rose100, Gfx.Center);
        Gfx.Text(g, _user.Id, Theme.Mono(11), new Rectangle(pill.Right + 6, 19, 160, 16), Theme.Rose200);

        // Name
        Gfx.Text(g, _user.Name, Theme.F(15, true), new Rectangle(120, 38, Width - 136, 22), Color.White);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        Cursor = MenuRect.Contains(e.Location) ? Cursors.Hand : Cursors.Default;
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        if (MenuRect.Contains(e.Location)) MenuClicked?.Invoke();
    }
}
