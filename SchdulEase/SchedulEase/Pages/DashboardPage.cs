namespace SchedulEase.Pages;

/// <summary>Dashboard: greeting, (teacher) live session, campus overview and one card per building.</summary>
public class DashboardPage : Panel
{
    public DashboardPage(PortalShell shell)
    {
        BackColor = Theme.Canvas;
        var body = new ScrollBody { Dock = DockStyle.Fill };
        Controls.Add(body);
        var c = body.Content;
        var user = shell.User;
        bool teacher = user.Role == UserRole.Teacher;
        int y = 18;

        Ui.Text(c, $"Hello, {user.FirstName}!", Theme.Pad, y, Theme.CardW, 28, 20, Theme.TextPrimary, true);
        y += 28;
        Ui.Text(c, teacher ? "Check live room availability and manage your classes." : "Find open classrooms and see what's in session.",
                Theme.Pad, y, Theme.CardW, 18, 12.5f, Theme.TextSecondary);
        y += 32;

        // Teacher: current live session (if any)
        if (teacher)
        {
            var mine = Campus.SessionOfTeacher(user.Id);
            if (mine != null)
            {
                Ui.Section(c, "Your Live Session", "Now", ref y);
                var card = new ActiveSessionCard(mine, () =>
                {
                    Campus.EndSession(user.Id);
                    shell.Toast("Session ended. The room is available again.");
                    shell.Navigate(PortalPage.Dashboard);
                }) { Location = new Point(Theme.Pad, y) };
                c.Controls.Add(card);
                y += card.Height + 10;
            }
        }

        // Campus overview
        Ui.Section(c, "Campus Overview", "All rooms", ref y);
        c.Controls.Add(new StatStrip { Location = new Point(Theme.Pad, y) });
        y += 56 + 16;

        // Buildings
        Ui.Section(c, "Buildings", $"{Campus.Buildings.Count} buildings", ref y);
        Ui.Text(c, "Tap a building to view its rooms.", Theme.Pad, y - 8, Theme.CardW, 16, 11.5f, Theme.TextSecondary);
        y += 16;
        foreach (var b in Campus.Buildings)
        {
            int id = b.Id;
            var card = new BuildingCard(b, () => shell.Navigate(PortalPage.Rooms, id)) { Location = new Point(Theme.Pad, y) };
            c.Controls.Add(card);
            y += card.Height + 10;
        }

        body.SetContentHeight(y + 20);
    }
}
