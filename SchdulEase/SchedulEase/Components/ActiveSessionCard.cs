namespace SchedulEase.Components;

/// <summary>Maroon "live session" banner with a countdown and a Clock Out button.</summary>
public class ActiveSessionCard : RoundedPanel
{
    readonly ClassSession _session;
    readonly TextLabel _timerLabel;
    readonly System.Windows.Forms.Timer _tick = new() { Interval = 1000 };

    public ActiveSessionCard(ClassSession session, Action onClockOut)
    {
        _session = session;
        Size = new Size(Theme.CardW, 150);
        Radius = 20;
        Fill = Theme.MaroonDark;
        FillTo = Theme.MaroonPrimary;
        Shadow = true;

        var room = Campus.FindRoom(session.RoomId);
        var building = Campus.FindBuilding(room.BuildingId);

        var live = PillLabel.Create("●  LIVE", Color.FromArgb(41, 255, 255, 255), Color.FromArgb(220, 252, 231), 10, 10);
        live.Location = new Point(14, 14);
        Controls.Add(live);

        _timerLabel = Ui.Text(this, "", Width - 14 - 150, 8, 150, 32, 20, Color.White, true, ContentAlignment.MiddleRight, true);

        Ui.Text(this, $"{session.ClassCode} · {session.Subject}", 14, 46, Width - 28, 20, 14, Color.White, true);
        Ui.Text(this, $"Section {session.Section} · {room.Name} · {building.Title}", 14, 68, Width - 28, 18, 11.5f, Theme.Rose100);
        Ui.Btn(this, "Clock Out & Release Room", 14, 96, Width - 28, 38, BtnLook.White, onClockOut, 12, 12);

        UpdateTimer();
        _tick.Tick += (_, _) => UpdateTimer();
        _tick.Start();
    }

    void UpdateTimer() => _timerLabel.Text = _session.Remaining.ToString(@"hh\:mm\:ss");

    protected override void Dispose(bool disposing)
    {
        if (disposing) _tick.Dispose();
        base.Dispose(disposing);
    }
}
