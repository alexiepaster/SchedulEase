namespace SchedulEase.Components;

/// <summary>One classroom row. Teachers get "Edit Room" (locked while occupied); students get "Details".</summary>
public class RoomCard : RoundedPanel
{
    public RoomCard(Classroom room, ClassSession session, bool canEdit, Action onAction)
    {
        Size = new Size(Theme.CardW, 128);
        Shadow = true;
        Border = Theme.BorderLight;

        var b = Campus.FindBuilding(room.BuildingId);
        Ui.Text(this, room.Name, 14, 12, 190, 22, 15, Theme.TextPrimary, true);
        Ui.Text(this, $"{b.Title} · {b.Name}", 14, 35, 220, 16, 11.5f, Theme.TextSecondary);

        var (bg, fg) = Theme.StatusColors(room.Status);
        var pill = PillLabel.Create(room.Status.ToString().ToUpper(), bg, fg);
        pill.Location = new Point(Width - 14 - pill.Width, 13);
        Controls.Add(pill);

        Ui.Text(this, $"{room.Capacity} seats  ·  {room.Equipment}", 14, 57, Width - 28, 18, 11.5f, Theme.TextSecondary);

        string hint = room.Status switch
        {
            RoomStatus.Occupied => session != null ? $"In use · {session.ClassCode} · until {session.EndTime:h:mm tt}" : "In use",
            RoomStatus.Reserved => "Reserved",
            _ => "Ready to use"
        };
        Ui.Text(this, hint, 14, 90, Width - 14 - 96 - 8 - 14, 30, 11.5f, fg, true, ContentAlignment.MiddleLeft);

        bool locked = canEdit && room.Status == RoomStatus.Occupied;
        string label = !canEdit ? "Details" : locked ? "Locked" : "Edit Room";
        var btn = Ui.Btn(this, label, Width - 14 - 96, 90, 96, 30, canEdit ? BtnLook.Soft : BtnLook.Solid, onAction, 10, 11.5f);
        btn.Enabled = !locked;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Theme.TileGray);
        e.Graphics.DrawLine(pen, 14, 84, Width - 14, 84);
    }
}
