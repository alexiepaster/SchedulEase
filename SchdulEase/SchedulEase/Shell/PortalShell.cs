namespace SchedulEase.Shell;

/// <summary>
/// The signed-in app frame: header, current page, hamburger drawer, bottom sheets and toast.
/// Pages call Navigate / Toast / OpenSheet on this class.
/// </summary>
public class PortalShell : Panel
{
    public User User { get; }
    public event Action LoggedOut;

    readonly HeaderBar _header;
    readonly Panel _host = new FlatPanel();
    readonly ToastBanner _toast = new ToastBanner();
    Control _page;
    DimOverlay _overlay;
    PortalPage _current;

    public PortalShell(User user)
    {
        User = user;
        BackColor = Theme.Canvas;

        _header = new HeaderBar(user) { Dock = DockStyle.Top };
        _header.MenuClicked += OpenMenu;
        _host.Dock = DockStyle.Fill;

        Controls.Add(_host);      // Fill control first, then the Top-docked header
        Controls.Add(_header);
        Controls.Add(_toast);

        Navigate(PortalPage.Dashboard);
    }

    // ---------- Navigation ----------
    public void Navigate(PortalPage page, int buildingId = 0)
    {
        CloseOverlay();
        _current = page;

        Control next;
        if (page == PortalPage.Rooms) next = new RoomsPage(this, buildingId);
        else if (page == PortalPage.Session) next = new SessionPage(this);
        else if (page == PortalPage.Classes) next = new ClassesPage(this);
        else next = new DashboardPage(this);
        next.Dock = DockStyle.Fill;

        var old = _page;
        _page = next;
        _host.Controls.Add(next);
        if (old != null)
        {
            _host.Controls.Remove(old);
            BeginInvoke(new Action(old.Dispose));   // dispose after the current click finishes
        }
        _toast.BringToFront();
    }

    public void Toast(string message)
    {
        _toast.Display(message);
    }

    // ---------- Hamburger drawer ----------
    void OpenMenu()
    {
        CloseOverlay();
        var items = User.Role == UserRole.Teacher
            ? new[]
              {
                  (PortalPage.Dashboard, "Dashboard", Glyph.Grid),
                  (PortalPage.Rooms, "Rooms", Glyph.Building),
                  (PortalPage.Session, "Class Session", Glyph.Clock),
              }
            : new[]
              {
                  (PortalPage.Dashboard, "Dashboard", Glyph.Grid),
                  (PortalPage.Rooms, "Rooms", Glyph.Building),
                  (PortalPage.Classes, "Active Classes", Glyph.Search),
              };

        _overlay = NewOverlay();
        var menu = new SideMenu(User, items, _current) { Width = 290, Height = _overlay.Height, Left = -290, Top = 0 };
        menu.PageChosen += p => BeginInvoke(new Action(() => Navigate(p)));
        menu.LogoutChosen += () => BeginInvoke(new Action(() => LoggedOut?.Invoke()));
        _overlay.Controls.Add(menu);

        var slide = new System.Windows.Forms.Timer { Interval = 12 };
        slide.Tick += (_, _) =>
        {
            if (menu.IsDisposed) { slide.Stop(); slide.Dispose(); return; }
            menu.Left = Math.Min(0, menu.Left + 48);
            if (menu.Left >= 0) { slide.Stop(); slide.Dispose(); }
        };
        slide.Start();
    }

    // ---------- Bottom sheet ----------
    /// <summary>Opens a white bottom sheet and returns it so the caller can add controls (390px wide).</summary>
    public RoundedPanel OpenSheet(int contentHeight)
    {
        CloseOverlay();
        _overlay = NewOverlay();

        var sheet = new RoundedPanel
        {
            Radius = 28,
            Fill = Color.White,
            Bounds = new Rectangle(0, _overlay.Height - contentHeight, Width, contentHeight + 40),   // bottom 40px hang off-screen so only the top corners show
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
        };
        _overlay.Controls.Add(sheet);

        var handle = new RoundedPanel { Radius = 2, Fill = Color.FromArgb(203, 213, 225), Bounds = new Rectangle((Width - 40) / 2, 10, 40, 4) };
        sheet.Controls.Add(handle);
        return sheet;
    }

    DimOverlay NewOverlay()
    {
        var snapshot = new Bitmap(Width, Height);
        DrawToBitmap(snapshot, new Rectangle(0, 0, Width, Height));

        var overlay = new DimOverlay(snapshot)
        {
            Bounds = new Rectangle(0, 0, Width, Height),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };
        overlay.Dismissed += CloseOverlay;
        Controls.Add(overlay);
        overlay.BringToFront();
        _toast.BringToFront();
        return overlay;
    }

    public void CloseOverlay()
    {
        if (_overlay == null) return;
        var o = _overlay;
        _overlay = null;
        Controls.Remove(o);
        BeginInvoke(new Action(o.Dispose));
    }
}
