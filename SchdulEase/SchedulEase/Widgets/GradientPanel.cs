namespace SchedulEase.Widgets;

/// <summary>Panel with a linear gradient background (2 or more colors).</summary>
public class GradientPanel : Panel
{
    public Color[] Colors { get; set; } = new[] { Theme.MaroonPrimary, Theme.MaroonDark };
    public float[] Positions { get; set; }
    public float Angle { get; set; } = 90f;   // 90 = top to bottom

    public GradientPanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        if (Width < 2 || Height < 2) return;
        using var br = new LinearGradientBrush(ClientRectangle, Colors[0], Colors[^1], Angle);
        if (Colors.Length > 2)
        {
            int n = Colors.Length;
            var pos = Positions ?? Enumerable.Range(0, n).Select(i => i / (n - 1f)).ToArray();
            br.InterpolationColors = new ColorBlend { Colors = Colors, Positions = pos };
        }
        e.Graphics.FillRectangle(br, ClientRectangle);
    }
}
