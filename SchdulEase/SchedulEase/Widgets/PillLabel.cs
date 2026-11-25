namespace SchedulEase.Widgets;

/// <summary>Small rounded badge, e.g. the AVAILABLE / OCCUPIED / RESERVED status pills.</summary>
public class PillLabel : TextLabel
{
    public Color Back { get; set; } = Theme.BorderLight;
    public Color Fore { get; set; } = Theme.TextSecondary;
    public float TextPx { get; set; } = 10.5f;

    public static PillLabel Create(string text, Color back, Color fore, float px = 10.5f, int padX = 10)
    {
        int w = TextRenderer.MeasureText(text, Theme.F(px, true), new Size(1000, 100), TextFormatFlags.NoPadding).Width + padX * 2;
        return new PillLabel { Text = text, Back = back, Fore = fore, TextPx = px, Size = new Size(w, (int)px + 11) };
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (Width < 2 || Height < 2) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Gfx.FillRound(e.Graphics, Back, new RectangleF(0, 0, Width, Height), Height / 2f);
        Gfx.Text(e.Graphics, Text, Theme.F(TextPx, true), ClientRectangle, Fore, Gfx.Center);
    }
}
