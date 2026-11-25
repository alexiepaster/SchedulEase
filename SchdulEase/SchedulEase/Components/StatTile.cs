namespace SchedulEase.Components;

/// <summary>Painted stat tiles: Total / Available / Occupied / Reserved.</summary>
public static class StatTile
{
    public static void Paint(Graphics g, RectangleF r, string number, string caption, Color bg, Color fg, float numPx)
    {
        Gfx.FillRound(g, bg, r, 12);
        var rc = Rectangle.Round(r);
        int numH = (int)numPx + 8;
        Gfx.Text(g, number, Theme.F(numPx, true), new Rectangle(rc.X, rc.Y + 5, rc.Width, numH), fg, Gfx.Center);
        Gfx.Text(g, caption, Theme.F(10.5f, true), new Rectangle(rc.X, rc.Y + 5 + numH, rc.Width, 14), fg, Gfx.Center);
    }

    public static void Row(Graphics g, RectangleF area, (int Total, int Available, int Occupied, int Reserved) s, float numPx)
    {
        const float gap = 8f;
        float w = (area.Width - gap * 3) / 4f;
        var tiles = new (string Caption, int Value, Color Bg, Color Fg)[]
        {
            ("Total", s.Total, Theme.TileGray, Theme.TextPrimary),
            ("Available", s.Available, Theme.AvailBg, Theme.AvailText),
            ("Occupied", s.Occupied, Theme.OccBg, Theme.OccText),
            ("Reserved", s.Reserved, Theme.ResBg, Theme.ResText),
        };
        for (int i = 0; i < tiles.Length; i++)
        {
            var r = new RectangleF(area.X + i * (w + gap), area.Y, w, area.Height);
            Paint(g, r, tiles[i].Value.ToString(), tiles[i].Caption, tiles[i].Bg, tiles[i].Fg, numPx);
        }
    }
}

/// <summary>A row of four stat tiles for the whole campus (dashboard overview).</summary>
public class StatStrip : Control
{
    readonly (int Total, int Available, int Occupied, int Reserved) _stats;

    public StatStrip()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        BackColor = Color.Transparent;
        _stats = Campus.Stats();
        Size = new Size(Theme.CardW, 56);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        StatTile.Row(e.Graphics, new RectangleF(0, 0, Width, Height), _stats, 20f);
    }
}
