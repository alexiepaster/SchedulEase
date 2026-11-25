namespace SchedulEase.Pages;

/// <summary>Teacher only: start a class session (occupy a room) or clock out of the current one.</summary>
public class SessionPage : Panel
{
    public SessionPage(PortalShell shell)
    {
        BackColor = Theme.Canvas;
        var body = new ScrollBody { Dock = DockStyle.Fill };
        Controls.Add(body);
        var c = body.Content;
        var teacher = shell.User;
        int y = 18;

        Ui.Text(c, "Class Session", Theme.Pad, y, Theme.CardW, 28, 20, Theme.TextPrimary, true);
        y += 28;
        Ui.Text(c, "Occupy a room for your class, then clock out when you're done.", Theme.Pad, y, Theme.CardW, 18, 12.5f, Theme.TextSecondary);
        y += 32;

        var mine = Campus.SessionOfTeacher(teacher.Id);
        if (mine != null)
        {
            Ui.Section(c, "Active Session", "Now", ref y);
            var card = new ActiveSessionCard(mine, () =>
            {
                Campus.EndSession(teacher.Id);
                shell.Toast("Session ended. The room is available again.");
                shell.Navigate(PortalPage.Session);
            }) { Location = new Point(Theme.Pad, y) };
            c.Controls.Add(card);
            y += card.Height + 6;
            Ui.Text(c, "Clock out to start another session.", Theme.Pad, y, Theme.CardW, 18, 12, Theme.TextSecondary);
            y += 30;
        }
        else
        {
            y = BuildForm(shell, c, y);
        }
        body.SetContentHeight(y + 20);
    }

    int BuildForm(PortalShell shell, Control c, int y)
    {
        Ui.Section(c, "Start Class Session", "Occupy room", ref y);

        var card = new RoundedPanel { Location = new Point(Theme.Pad, y), Width = Theme.CardW, Shadow = true, Border = Theme.BorderLight };
        const int ix = 14, iw = Theme.CardW - 28;
        int iy = 16;

        var code = Ui.LabeledField(card, "Class Code", "", ix, ref iy, iw, "e.g. CS-301A");
        var subject = Ui.LabeledField(card, "Subject", "", ix, ref iy, iw, "e.g. Object-Oriented Programming");
        var section = Ui.LabeledField(card, "Section", "", ix, ref iy, iw, "e.g. BSCS-3A");

        // Duration chips
        Ui.Text(card, "Duration", ix, iy, iw, 16, 12, Theme.TextPrimary, true);
        iy += 22;
        int minutes = 90;
        var chips = new List<(RoundedButton Button, int Minutes)>();
        void RefreshChips()
        {
            foreach (var (button, m) in chips)
            {
                button.Look = m == minutes ? BtnLook.ChipActive : BtnLook.Chip;
                button.Invalidate();
            }
        }
        int cw = (iw - 16) / 3, i = 0;
        foreach (int m in new[] { 60, 90, 120 })
        {
            int value = m;
            var chip = Ui.Btn(card, $"{m} mins", ix + i * (cw + 8), iy, cw, 38, BtnLook.Chip, () => { minutes = value; RefreshChips(); }, 12, 12);
            chips.Add((chip, m));
            i++;
        }
        RefreshChips();
        iy += 38 + 16;

        // Room picker (available rooms only)
        Ui.Text(card, "Room", ix, iy, iw, 16, 12, Theme.TextPrimary, true);
        var available = Campus.Rooms.Where(r => r.Status == RoomStatus.Available).OrderBy(r => r.BuildingId).ToList();
        var roomPicker = FieldBox.Dropdown();
        foreach (var r in available)
            roomPicker.Combo.Items.Add($"{Campus.FindBuilding(r.BuildingId).Title} · {r.Name}  ({r.Capacity} seats)");
        if (available.Count > 0) roomPicker.Combo.SelectedIndex = 0;
        Ui.Field(card, roomPicker, ix, iy + 22, iw);
        iy += 70;

        void Occupy()
        {
            if (roomPicker.Combo.SelectedIndex < 0) { shell.Toast("No available room to occupy."); return; }
            string error = Campus.StartSession(shell.User, code.Box.Text, subject.Box.Text, section.Box.Text,
                                               minutes, available[roomPicker.Combo.SelectedIndex].Id);
            if (error != null) { shell.Toast(error); return; }
            shell.Toast("Room occupied. Your session is now live.");
            shell.Navigate(PortalPage.Session);
        }

        var go = Ui.Btn(card, "Occupy Room", ix, iy, iw, 46, BtnLook.Gradient, Occupy, 16, 13.5f);
        go.Enabled = available.Count > 0;
        iy += 46 + 14;

        card.Height = iy + 5;
        c.Controls.Add(card);
        return y + card.Height + 12;
    }
}
