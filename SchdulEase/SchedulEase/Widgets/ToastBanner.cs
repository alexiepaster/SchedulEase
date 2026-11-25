namespace SchedulEase.Widgets;

/// <summary>Dark pop-up message at the top of the screen (the prototype's .toast-banner).</summary>
public class ToastBanner : Control
{
    readonly System.Windows.Forms.Timer _timer = new() { Interval = 2400 };

    public ToastBanner()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        BackColor = Color.Transparent;
        Visible = false;
        _timer.Tick += (_, _) => { _timer.Stop(); Visible = false; };
    }

    public void Display(string message)
    {
        Text = message;
        SetBounds(16, 12, Theme.PageWidth - 32, 40);
        Visible = true;
        BringToFront();
        _timer.Stop();
        _timer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Gfx.FillRound(e.Graphics, Theme.TextPrimary, new RectangleF(0, 0, Width - 1, Height - 1), 14);
        Gfx.Text(e.Graphics, Text, Theme.F(12, true), new Rectangle(12, 0, Width - 24, Height), Color.White, Gfx.Center | TextFormatFlags.EndEllipsis);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }
}
