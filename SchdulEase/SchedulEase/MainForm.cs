namespace SchedulEase;

/// <summary>The "phone" window. Switches between Role Select, Login and the signed-in portal.</summary>
public class MainForm : Form
{
    public MainForm()
    {
        Text = "SchedulEase";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Theme.MaroonDark;
        DoubleBuffered = true;

        int height = Math.Min(780, Screen.PrimaryScreen.WorkingArea.Height - 80);
        ClientSize = new Size(Theme.PageWidth, height);

        ShowRoleSelect();
    }

    // Reduces flicker when many child controls repaint.
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
            return cp;
        }
    }

    void SwapPage(Control page)
    {
        var old = Controls.Cast<Control>().ToList();
        page.Dock = DockStyle.Fill;
        Controls.Add(page);
        foreach (var c in old)
        {
            Controls.Remove(c);
            c.Dispose();
        }
    }

    // Handlers run from inside the old page's click events, so swap pages after the click finishes.
    void Go(Action action) => BeginInvoke(action);

    void ShowRoleSelect()
    {
        var page = new RoleSelectPage();
        page.RoleChosen += role => Go(() => ShowLogin(role));
        SwapPage(page);
    }

    void ShowLogin(UserRole role)
    {
        var page = new LoginPage(role);
        page.Back += () => Go(ShowRoleSelect);
        page.LoggedIn += user => Go(() => ShowPortal(user));
        SwapPage(page);
    }

    void ShowPortal(User user)
    {
        var shell = new PortalShell(user);
        shell.LoggedOut += ShowRoleSelect;   // PortalShell already defers this call
        SwapPage(shell);
    }
}
