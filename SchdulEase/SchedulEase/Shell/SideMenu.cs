namespace SchedulEase.Shell;

public enum PortalPage { Dashboard, Rooms, Session, Classes }

/// <summary>Hamburger drawer: brand, signed-in user, page links and Logout.</summary>
public class SideMenu : Panel
{
    readonly User _user;
    readonly (PortalPage Page, string Label, Glyph Icon)[] _items;
    readonly PortalPage _active;
    int _hover = -2;   // -2 = none, -1 = logout, 0.. = menu item

    public event Action<PortalPage> PageChosen;
    public event Action LogoutChosen;

    public SideMenu(User user, (PortalPage Page, string Label, Glyph Icon)[] items, PortalPage active)
    {
        _user = user;
        _items = items;
        _active = active;
        DoubleBuffered = true;
    }

    Rectangle ItemRect(int i) => new Rectangle(14, 180 + i * 58, Width - 28, 50);
    Rectangle LogoutRect => new Rectangle(14, Height - 76, Width - 28, 50);

    int HitTest(Point p)
    {
        for (int i = 0; i < _items.Length; i++)
            if (ItemRect(i).Contains(p)) return i;
        return LogoutRect.Contains(p) ? -1 : -2;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        if (Width < 2 || Height < 2) return;

        using (var bg = new LinearGradientBrush(ClientRectangle, Theme.MaroonPrimary, Theme.MaroonDeep, 90f))
            g.FillRectangle(bg, ClientRectangle);

        // Brand
        Gfx.FillRound(g, Color.White, new RectangleF(20, 30, 46, 46), 16);
        Icons.Draw(g, Glyph.Calendar, new RectangleF(30, 40, 26, 26), Theme.MaroonPrimary, 2.2f);
        Gfx.Text(g, "SchedulEase", Theme.F(20, true), new Rectangle(78, 30, 200, 26), Color.White);
        Gfx.Text(g, "Room Locator", Theme.F(11.5f), new Rectangle(78, 56, 200, 18), Theme.Rose200);

        // Signed-in user card
        var card = new RectangleF(14.5f, 98.5f, Width - 29, 64);
        Gfx.FillRound(g, Color.FromArgb(30, 255, 255, 255), card, 16);
        Gfx.DrawRound(g, Color.FromArgb(40, 255, 255, 255), card, 16);
        var avatar = new Rectangle(26, 110, 40, 40);
        Gfx.FillRound(g, Color.White, avatar, 14);
        Gfx.Text(g, _user.Initials, Theme.F(14, true), avatar, Theme.MaroonPrimary, Gfx.Center);
        Gfx.Text(g, _user.Name, Theme.F(14, true), new Rectangle(76, 111, Width - 96, 20), Color.White);
        Gfx.Text(g, $"{_user.Role} · {_user.Id}", Theme.F(11), new Rectangle(76, 131, Width - 96, 18), Theme.Rose200);

        // Menu items
        for (int i = 0; i < _items.Length; i++)
        {
            var r = ItemRect(i);
            bool active = _items[i].Page == _active;
            if (active) Gfx.FillRound(g, Color.White, r, 16);
            else if (_hover == i) Gfx.FillRound(g, Color.FromArgb(30, 255, 255, 255), r, 16);

            var fore = active ? Theme.MaroonPrimary : Theme.Rose100;
            Icons.Draw(g, _items[i].Icon, new RectangleF(r.X + 16, r.Y + 14, 22, 22), fore, 2f);
            Gfx.Text(g, _items[i].Label, Theme.F(14, true), new Rectangle(r.X + 50, r.Y, r.Width - 60, r.Height), fore);
        }

        // Logout
        var lr = LogoutRect;
        Gfx.FillRound(g, Color.FromArgb(_hover == -1 ? 56 : 30, 255, 255, 255), lr, 16);
        Icons.Draw(g, Glyph.Logout, new RectangleF(lr.X + 16, lr.Y + 14, 22, 22), Color.White, 2f);
        Gfx.Text(g, "Logout", Theme.F(14, true), new Rectangle(lr.X + 50, lr.Y, lr.Width - 60, lr.Height), Color.White);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int hit = HitTest(e.Location);
        if (hit != _hover) { _hover = hit; Invalidate(); }
        Cursor = hit != -2 ? Cursors.Hand : Cursors.Default;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hover = -2;
        Invalidate();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        int hit = HitTest(e.Location);
        if (hit >= 0) PageChosen?.Invoke(_items[hit].Page);
        else if (hit == -1) LogoutChosen?.Invoke();
    }
}
