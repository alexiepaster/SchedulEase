namespace SchedulEase.Pages;

/// <summary>
/// Rooms panel: filter by building, search, and (teachers only) edit a room.
/// Occupied rooms are locked and cannot be edited.
/// </summary>
public class RoomsPage : Panel
{
    readonly PortalShell _shell;
    readonly ScrollBody _body;
    readonly FlatPanel _list = new FlatPanel();
    readonly FieldBox _search;
    readonly List<(RoundedButton Button, int BuildingId)> _chips = new();
    readonly bool _canEdit;
    readonly int _listTop;
    int _filter;

    public RoomsPage(PortalShell shell, int buildingId)
    {
        _shell = shell;
        _filter = buildingId;
        _canEdit = shell.User.Role == UserRole.Teacher;
        BackColor = Theme.Canvas;

        _body = new ScrollBody { Dock = DockStyle.Fill };
        Controls.Add(_body);
        var c = _body.Content;
        int y = 18;

        Ui.Text(c, "Rooms", Theme.Pad, y, Theme.CardW, 28, 20, Theme.TextPrimary, true);
        y += 28;
        Ui.Text(c, _canEdit ? "View and edit rooms. Occupied rooms are locked." : "Browse classrooms and check their live status.",
                Theme.Pad, y, Theme.CardW, 18, 12.5f, Theme.TextSecondary);
        y += 30;

        _search = Ui.Field(c, FieldBox.TextInput("Search room or equipment..."), Theme.Pad, y, Theme.CardW);
        y += 52;

        // Building filter chips
        var options = new List<(string Label, int Id)> { ("All Buildings", 0) };
        options.AddRange(Campus.Buildings.Select(b => (b.Title, b.Id)));
        int x = Theme.Pad;
        foreach (var (label, id) in options)
        {
            int w = TextRenderer.MeasureText(label, Theme.F(11.5f, true), new Size(500, 40), TextFormatFlags.NoPadding).Width + 26;
            int bid = id;
            var chip = Ui.Btn(c, label, x, y, w, 30, BtnLook.Chip, () => { _filter = bid; RefreshChips(); RebuildList(); }, 15, 11.5f);
            _chips.Add((chip, id));
            x += w + 6;
        }
        y += 42;

        _list.SetBounds(0, y, Theme.PageWidth, 60);
        c.Controls.Add(_list);
        _listTop = y;

        RefreshChips();
        RebuildList();
        _search.Box.TextChanged += (_, _) => RebuildList();
    }

    void RefreshChips()
    {
        foreach (var (button, id) in _chips)
        {
            button.Look = id == _filter ? BtnLook.ChipActive : BtnLook.Chip;
            button.Invalidate();
        }
    }

    void RebuildList()
    {
        foreach (var ctl in _list.Controls.Cast<Control>().ToList()) ctl.Dispose();

        string q = _search.Box.Text.Trim();
        var rooms = Campus.Rooms
            .Where(r => (_filter == 0 || r.BuildingId == _filter)
                        && (q.Length == 0
                            || r.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                            || r.Equipment.Contains(q, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        int y = 0;
        foreach (var room in rooms)
        {
            var r = room;
            var card = new RoomCard(r, Campus.SessionOfRoom(r.Id), _canEdit, () => OnRoomAction(r)) { Location = new Point(Theme.Pad, y) };
            _list.Controls.Add(card);
            y += card.Height + 8;
        }
        if (rooms.Count == 0)
            Ui.Text(_list, "No rooms match your search.", 0, 24, Theme.PageWidth, 20, 12.5f, Theme.TextSecondary, false, ContentAlignment.TopCenter);

        _list.Height = Math.Max(y, 70);
        _body.SetContentHeight(_listTop + _list.Height + 20);
    }

    void OnRoomAction(Classroom room)
    {
        if (!_canEdit) { RoomSheets.ShowDetails(_shell, room); return; }

        if (room.Status == RoomStatus.Occupied)   // safety net: the button is already disabled
        {
            _shell.Toast("This room is occupied, so it can't be edited.");
            return;
        }
        RoomSheets.ShowEdit(_shell, room, RebuildList);
    }
}
