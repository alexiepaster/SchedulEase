namespace SchedulEase.Components;

/// <summary>One live class session (student "Active Classes" list).</summary>
public class ClassCard : RoundedPanel
{
    public ClassCard(ClassSession s)
    {
        Size = new Size(Theme.CardW, 112);
        Shadow = true;
        Border = Theme.BorderLight;

        var room = Campus.FindRoom(s.RoomId);
        var b = Campus.FindBuilding(room.BuildingId);

        Ui.Text(this, s.ClassCode, 14, 12, 200, 22, 15, Theme.TextPrimary, true);
        var live = PillLabel.Create("LIVE", Theme.AvailBg, Theme.AvailText);
        live.Location = new Point(Width - 14 - live.Width, 13);
        Controls.Add(live);

        Ui.Text(this, s.Subject, 14, 36, Width - 28, 18, 12.5f, Theme.TextSecondary);
        Ui.Text(this, $"{room.Name} · {b.Title} · Section {s.Section}", 14, 59, Width - 28, 18, 11.5f, Theme.MaroonPrimary, true);
        Ui.Text(this, $"{s.TeacherName} · until {s.EndTime:h:mm tt}", 14, 79, Width - 28, 18, 11.5f, Theme.TextSecondary);
    }
}
