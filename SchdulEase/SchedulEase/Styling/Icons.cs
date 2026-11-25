namespace SchedulEase.Styling;

public enum Glyph { Menu, Grid, Building, Clock, Search, Logout, Person, Cap, Calendar, Lock, Arrow }

/// <summary>Line icons drawn with GDI+ on a 24x24 grid (same style as the prototype's SVG icons).</summary>
public static class Icons
{
    public static void Draw(Graphics g, Glyph glyph, RectangleF r, Color color, float stroke = 2f)
    {
        var state = g.Save();
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TranslateTransform(r.X, r.Y);
        g.ScaleTransform(r.Width / 24f, r.Height / 24f);
        using var pen = new Pen(color, stroke) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };

        switch (glyph)
        {
            case Glyph.Menu:
                g.DrawLine(pen, 3f, 6f, 21f, 6f);
                g.DrawLine(pen, 3f, 12f, 21f, 12f);
                g.DrawLine(pen, 3f, 18f, 21f, 18f);
                break;
            case Glyph.Grid:
                g.DrawRectangle(pen, 3f, 3f, 7f, 7f);
                g.DrawRectangle(pen, 14f, 3f, 7f, 7f);
                g.DrawRectangle(pen, 3f, 14f, 7f, 7f);
                g.DrawRectangle(pen, 14f, 14f, 7f, 7f);
                break;
            case Glyph.Building:
                g.DrawRectangle(pen, 5f, 3f, 14f, 18f);
                g.DrawLine(pen, 9f, 8f, 15f, 8f);
                g.DrawLine(pen, 9f, 12f, 15f, 12f);
                g.DrawRectangle(pen, 10f, 16f, 4f, 5f);
                break;
            case Glyph.Clock:
                g.DrawEllipse(pen, 3f, 3f, 18f, 18f);
                g.DrawLine(pen, 12f, 7f, 12f, 12f);
                g.DrawLine(pen, 12f, 12f, 15.5f, 14f);
                break;
            case Glyph.Search:
                g.DrawEllipse(pen, 4f, 4f, 12f, 12f);
                g.DrawLine(pen, 13.5f, 13.5f, 20f, 20f);
                break;
            case Glyph.Logout:
                g.DrawLines(pen, new[] { new PointF(10, 3), new PointF(4, 3), new PointF(4, 21), new PointF(10, 21) });
                g.DrawLine(pen, 10f, 12f, 20f, 12f);
                g.DrawLines(pen, new[] { new PointF(16, 8), new PointF(20, 12), new PointF(16, 16) });
                break;
            case Glyph.Person:
                g.DrawEllipse(pen, 8f, 3f, 8f, 8f);
                g.DrawArc(pen, 4f, 14f, 16f, 14f, 180f, 180f);
                g.DrawLine(pen, 4f, 21f, 20f, 21f);
                break;
            case Glyph.Cap:
                g.DrawPolygon(pen, new[] { new PointF(12, 4), new PointF(23, 9), new PointF(12, 14), new PointF(1, 9) });
                g.DrawLines(pen, new[] { new PointF(6, 11.5f), new PointF(6, 16.5f), new PointF(12, 19.5f), new PointF(18, 16.5f), new PointF(18, 11.5f) });
                break;
            case Glyph.Calendar:
                using (var p = Gfx.Round(new RectangleF(3, 5, 18, 16), 3)) g.DrawPath(pen, p);
                g.DrawLine(pen, 3f, 10f, 21f, 10f);
                g.DrawLine(pen, 8f, 2.5f, 8f, 6.5f);
                g.DrawLine(pen, 16f, 2.5f, 16f, 6.5f);
                break;
            case Glyph.Lock:
                using (var p = Gfx.Round(new RectangleF(5, 11, 14, 10), 2)) g.DrawPath(pen, p);
                g.DrawArc(pen, 8f, 3f, 8f, 10f, 180f, 180f);
                g.DrawLine(pen, 8f, 8f, 8f, 11f);
                g.DrawLine(pen, 16f, 8f, 16f, 11f);
                break;
            case Glyph.Arrow:
                g.DrawLine(pen, 4f, 12f, 20f, 12f);
                g.DrawLines(pen, new[] { new PointF(14, 6), new PointF(20, 12), new PointF(14, 18) });
                break;
        }
        g.Restore(state);
    }
}
