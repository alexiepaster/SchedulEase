namespace SchedulEase.Widgets;

/// <summary>Card container: rounded corners, optional border, gradient fill and soft shadow.</summary>
public class RoundedPanel : Panel
{
    public int Radius { get; set; } = 18;
    public Color Fill { get; set; } = Color.White;
    public Color FillTo { get; set; } = Color.Empty;   // set for a gradient
    public Color Border { get; set; } = Color.Empty;
    public bool Shadow { get; set; }                    // uses the bottom 5px of the control for the shadow

    public RoundedPanel()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);
        BackColor = Color.Transparent;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (Width < 4 || Height < 8) return;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var r = new RectangleF(0.5f, 0.5f, Width - 1, Height - 1 - (Shadow ? 5 : 0));

        if (Shadow)
            for (int i = 1; i <= 4; i++)
                Gfx.FillRound(g, Color.FromArgb(7, 15, 23, 42), new RectangleF(r.X, r.Y + i, r.Width, r.Height), Radius);

        if (FillTo != Color.Empty)
        {
            using var br = Gfx.Gradient(r, Fill, FillTo, 45f);
            Gfx.FillWith(g, br, r, Radius);
        }
        else Gfx.FillRound(g, Fill, r, Radius);

        if (Border != Color.Empty) Gfx.DrawRound(g, Border, r, Radius);
        base.OnPaint(e);
    }
}
