namespace SchedulEase.Components;

/// <summary>Bottom sheets for a room: Edit (teachers) and Details (students).</summary>
public static class RoomSheets
{
    public static void ShowEdit(PortalShell shell, Classroom room, Action onSaved)
    {
        var sheet = shell.OpenSheet(486);
        var b = Campus.FindBuilding(room.BuildingId);

        Ui.Text(sheet, "Edit Room", 22, 28, 346, 26, 18, Theme.TextPrimary, true);
        Ui.Text(sheet, $"{b.Title} · {b.Name}", 22, 55, 346, 18, 12, Theme.TextSecondary);

        int y = 88;
        var name = Ui.LabeledField(sheet, "Room Name", room.Name, 22, ref y, 346);
        var cap = Ui.LabeledField(sheet, "Capacity (seats)", room.Capacity.ToString(), 22, ref y, 346);
        var eq = Ui.LabeledField(sheet, "Equipment", room.Equipment, 22, ref y, 346);

        Ui.Text(sheet, "Status", 22, y, 346, 16, 12, Theme.TextPrimary, true);
        y += 22;

        var status = room.Status;
        RoundedButton available = null, reserved = null;
        void RefreshChips()
        {
            available.Look = status == RoomStatus.Available ? BtnLook.ChipActive : BtnLook.Chip;
            reserved.Look = status == RoomStatus.Reserved ? BtnLook.ChipActive : BtnLook.Chip;
            available.Invalidate();
            reserved.Invalidate();
        }
        available = Ui.Btn(sheet, "Available", 22, y, 169, 38, BtnLook.Chip, () => { status = RoomStatus.Available; RefreshChips(); }, 12, 12.5f);
        reserved = Ui.Btn(sheet, "Reserved", 199, y, 169, 38, BtnLook.Chip, () => { status = RoomStatus.Reserved; RefreshChips(); }, 12, 12.5f);
        RefreshChips();
        y += 54;

        void Save()
        {
            if (!int.TryParse(cap.Box.Text.Trim(), out int capacity))
            {
                shell.Toast("Capacity must be a number.");
                return;
            }
            string error = Campus.UpdateRoom(room.Id, name.Box.Text, capacity, eq.Box.Text, status);
            if (error != null) { shell.Toast(error); return; }
            shell.CloseOverlay();
            shell.Toast("Room updated.");
            onSaved();
        }

        Ui.Btn(sheet, "Save Changes", 22, y, 346, 46, BtnLook.Gradient, Save, 16, 13.5f);
        y += 54;
        Ui.Btn(sheet, "Cancel", 22, y, 346, 38, BtnLook.Soft, shell.CloseOverlay, 14, 12.5f);
    }

    public static void ShowDetails(PortalShell shell, Classroom room)
    {
        var session = Campus.SessionOfRoom(room.Id);
        var sheet = shell.OpenSheet(session != null ? 440 : 300);
        var b = Campus.FindBuilding(room.BuildingId);

        Ui.Text(sheet, room.Name, 22, 28, 230, 26, 18, Theme.TextPrimary, true);
        Ui.Text(sheet, $"{b.Title} · {b.Name}", 22, 55, 250, 18, 12, Theme.TextSecondary);
        var (bg, fg) = Theme.StatusColors(room.Status);
        var pill = PillLabel.Create(room.Status.ToString().ToUpper(), bg, fg);
        pill.Location = new Point(Theme.PageWidth - 22 - pill.Width, 32);
        sheet.Controls.Add(pill);

        int y = 92;
        Ui.Row(sheet, "Capacity", $"{room.Capacity} seats", 22, ref y, 346);
        Ui.Row(sheet, "Equipment", room.Equipment, 22, ref y, 346);
        if (session != null)
        {
            Ui.Row(sheet, "Class", $"{session.ClassCode} · {session.Subject}", 22, ref y, 346);
            Ui.Row(sheet, "Section", session.Section, 22, ref y, 346);
            Ui.Row(sheet, "Teacher", session.TeacherName, 22, ref y, 346);
            Ui.Row(sheet, "Ends at", session.EndTime.ToString("h:mm tt"), 22, ref y, 346);
        }
        Ui.Btn(sheet, "Close Room Details", 22, y + 6, 346, 46, BtnLook.Gradient, shell.CloseOverlay, 16, 13.5f);
    }
}
