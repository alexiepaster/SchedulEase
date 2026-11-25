namespace SchedulEase.Widgets;

public enum BtnLook { Gradient, White, Solid, Soft, Glass, Chip, ChipActive }

/// <summary>Owner-drawn rounded button matching the prototype's button styles.</summary>
public class RoundedButton : Control
{
    public BtnLook Look { get; set; } = BtnLook.Gradient;
    public int Radius { get; set; } = 14;
    public float TextPx { get; set; } = 13f;

    bool _hover, _down;

    public RoundedButton()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);
        BackColor = Color.Transparent;
        Cursor = Cursors.Hand;
        TabStop = false;
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; _down = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { _down = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { _down = false; Invalidate(); base.OnMouseUp(e); }
    protected override void OnEnabledChanged(EventArgs e) { Cursor = Enabled ? Cursors.Hand : Cursors.Default; Invalidate(); base.OnEnabledChanged(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (Width < 4 || Height < 4) return;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var r = new RectangleF(0.5f, 0.5f, Width - 1, Height - 1);
        Color fore = Color.White;
        bool dark = false;

        if (!Enabled)
        {
            Gfx.FillRound(g, Theme.BorderLight, r, Radius);
            fore = Theme.TextMuted;
        }
        else
        {
            switch (Look)
            {
                case BtnLook.Gradient:
                    using (var br = Gfx.Gradient(r, Theme.MaroonPrimary, Theme.MaroonDark, 45f)) Gfx.FillWith(g, br, r, Radius);
                    dark = true;
                    break;
                case BtnLook.Solid:
                    Gfx.FillRound(g, Theme.MaroonPrimary, r, Radius);
                    dark = true;
                    break;
                case BtnLook.White:
                    Gfx.FillRound(g, Color.White, r, Radius);
                    fore = Theme.MaroonPrimary;
                    break;
                case BtnLook.Soft:
                    Gfx.FillRound(g, Theme.MaroonSoft, r, Radius);
                    Gfx.DrawRound(g, Theme.MaroonBorder, r, Radius);
                    fore = Theme.MaroonPrimary;
                    break;
                case BtnLook.Glass:
                    Gfx.FillRound(g, Color.FromArgb(36, 255, 255, 255), r, Radius);
                    Gfx.DrawRound(g, Color.FromArgb(51, 255, 255, 255), r, Radius);
                    break;
                case BtnLook.Chip:
                    Gfx.FillRound(g, Color.White, r, Radius);
                    Gfx.DrawRound(g, Theme.BorderLight, r, Radius);
                    fore = Theme.TextSecondary;
                    break;
                case BtnLook.ChipActive:
                    Gfx.FillRound(g, Theme.MaroonPrimary, r, Radius);
                    dark = true;
                    break;
            }

            if (_hover) Gfx.FillRound(g, dark || Look == BtnLook.Glass ? Color.FromArgb(24, 255, 255, 255) : Color.FromArgb(14, 135, 31, 42), r, Radius);
            if (_down) Gfx.FillRound(g, Color.FromArgb(28, 0, 0, 0), r, Radius);
        }

        Gfx.Text(g, Text, Theme.F(TextPx, true), ClientRectangle, fore, Gfx.Center);
    }
}
