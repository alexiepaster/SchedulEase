namespace SchedulEase.Pages;

/// <summary>Student only: search the classes that are in session right now.</summary>
public class ClassesPage : Panel
{
    readonly ScrollBody _body;
    readonly FlatPanel _list = new FlatPanel();
    readonly FieldBox _search;
    readonly int _listTop;

    public ClassesPage(PortalShell shell)
    {
        BackColor = Theme.Canvas;
        _body = new ScrollBody { Dock = DockStyle.Fill };
        Controls.Add(_body);
        var c = _body.Content;
        int y = 18;

        Ui.Text(c, "Active Classes", Theme.Pad, y, Theme.CardW, 28, 20, Theme.TextPrimary, true);
        y += 28;
        Ui.Text(c, "See which classes are in session and where to find them.", Theme.Pad, y, Theme.CardW, 18, 12.5f, Theme.TextSecondary);
        y += 30;

        _search = Ui.Field(c, FieldBox.TextInput("Search class code, subject, room or teacher..."), Theme.Pad, y, Theme.CardW);
        y += 56;

        _list.SetBounds(0, y, Theme.PageWidth, 60);
        c.Controls.Add(_list);
        _listTop = y;

        RebuildList();
        _search.Box.TextChanged += (_, _) => RebuildList();
    }

    void RebuildList()
    {
        foreach (var ctl in _list.Controls.Cast<Control>().ToList()) ctl.Dispose();

        string q = _search.Box.Text.Trim();
        bool Match(string s) => s.Contains(q, StringComparison.OrdinalIgnoreCase);

        var sessions = Campus.Sessions
            .Where(s => q.Length == 0 || Match(s.ClassCode) || Match(s.Subject) || Match(s.Section)
                        || Match(s.TeacherName) || Match(Campus.FindRoom(s.RoomId).Name))
            .ToList();

        int y = 0;
        foreach (var s in sessions)
        {
            var card = new ClassCard(s) { Location = new Point(Theme.Pad, y) };
            _list.Controls.Add(card);
            y += card.Height + 8;
        }
        if (sessions.Count == 0)
            Ui.Text(_list, "No active classes found.", 0, 24, Theme.PageWidth, 20, 12.5f, Theme.TextSecondary, false, ContentAlignment.TopCenter);

        _list.Height = Math.Max(y, 70);
        _body.SetContentHeight(_listTop + _list.Height + 20);
    }
}
