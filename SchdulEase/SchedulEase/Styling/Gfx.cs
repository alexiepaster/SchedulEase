namespace SchedulEase.Styling;

/// <summary>Small drawing helpers: rounded rectangles, gradients and text.</summary>
public static class Gfx
{
    public const TextFormatFlags Center = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;
    public const TextFormatFlags Left = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis;

    public static GraphicsPath Round(RectangleF r, float radius)
    {
        var p = new GraphicsPath();
        float d = Math.Min(radius * 2f, Math.Min(r.Width, r.Height));
        if (d <= 0.5f) { p.AddRectangle(r); return p; }
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    public static void FillRound(Graphics g, Color c, RectangleF r, float radius)
    {
        using var b = new SolidBrush(c);
        using var p = Round(r, radius);
        g.FillPath(b, p);
    }

    public static void FillWith(Graphics g, Brush b, RectangleF r, float radius)
    {
        using var p = Round(r, radius);
        g.FillPath(b, p);
    }

    public static void DrawRound(Graphics g, Color c, RectangleF r, float radius, float width = 1f)
    {
        using var pen = new Pen(c, width);
        using var p = Round(r, radius);
        g.DrawPath(pen, p);
    }

    public static LinearGradientBrush Gradient(RectangleF r, Color a, Color b, float angle)
        => new LinearGradientBrush(r, a, b, angle);

    public static void Text(Graphics g, string text, Font font, Rectangle r, Color color, TextFormatFlags flags = Left)
        => TextRenderer.DrawText(g, text, font, r, color, flags);
}
