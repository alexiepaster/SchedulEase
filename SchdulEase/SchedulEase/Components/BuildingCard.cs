namespace SchedulEase.Components;

/// <summary>
/// Dashboard card for one building: total rooms, how many are available / occupied / reserved,
/// and a "View Details" button that opens the Rooms page for that building.
/// </summary>
public class BuildingCard : RoundedPanel
{
    readonly (int Total, int Available, int Occupied, int Reserved) _s;

    public BuildingCard(Building b, Action onOpen)
    {
        _s = Campus.Stats(b.Id);
        Size = new Size(Theme.CardW, 196);
        Shadow = true;
        Border = Theme.BorderLight;
        Cursor = Cursors.Hand;

        Ui.Text(this, b.Title, 68, 14, 160, 22, 15, Theme.TextPrimary, true);
        Ui.Text(this, b.Name, 68, 37, 190, 18, 11.5f, Theme.TextSecondary);

        int pct = _s.Total == 0 ? 0 : (int)Math.Round(100.0 * _s.Occupied / _s.Total);
        var (bg, fg) = pct < 50 ? (Theme.AvailBg, Theme.AvailText) : pct < 80 ? (Theme.OccBg, Theme.OccText) : (Theme.MaroonSoft, Theme.MaroonPrimary);
        var pill = PillLabel.Create($"{pct}% in use", bg, fg);
        pill.Location = new Point(Width - 14 - pill.Width, 24);
        Controls.Add(pill);

        Ui.Btn(this, "View Details  →", 14, 146, Width - 28, 34, BtnLook.Soft, onOpen, 12, 12.5f);
        Click += (_, _) => onOpen();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Icon tile
        Gfx.FillRound(g, Theme.MaroonSoft, new RectangleF(14, 14, 44, 44), 14);
        Icons.Draw(g, Glyph.Building, new RectangleF(25, 25, 22, 22), Theme.MaroonPrimary, 2f);

        // Stat tiles
        StatTile.Row(g, new RectangleF(14, 70, Width - 28, 50), _s, 18f);

        // Occupancy bar
        var track = new RectangleF(14, 130, Width - 28, 6);
        using (var path = Gfx.Round(track, 3))
        {
            g.SetClip(path);
            using (var bg = new SolidBrush(Theme.BorderLight)) g.FillRectangle(bg, track);
            if (_s.Total > 0)
            {
                float x = track.X;
                void Seg(int count, Color c)
                {
                    float w = track.Width * count / _s.Total;
                    using var br = new SolidBrush(c);
                    g.FillRectangle(br, x, track.Y, w, track.Height);
                    x += w;
                }
                Seg(_s.Occupied, Color.FromArgb(245, 158, 11));
                Seg(_s.Reserved, Color.FromArgb(59, 130, 246));
                Seg(_s.Available, Color.FromArgb(34, 197, 94));
            }
            g.ResetClip();
        }
    }
}
