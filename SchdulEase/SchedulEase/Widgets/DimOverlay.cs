namespace SchedulEase.Widgets;

/// <summary>
/// Dark backdrop (the prototype's .modal-backdrop). It paints a snapshot of the screen underneath
/// with a dark tint on top, so side menus and bottom sheets can sit above it.
/// </summary>
public class DimOverlay : Panel
{
    readonly Bitmap _snapshot;
    public event Action Dismissed;

    public DimOverlay(Bitmap snapshot)
    {
        _snapshot = snapshot;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (_snapshot != null) e.Graphics.DrawImageUnscaled(_snapshot, 0, 0);
        using var tint = new SolidBrush(Color.FromArgb(140, 15, 23, 42));
        e.Graphics.FillRectangle(tint, ClientRectangle);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        Dismissed?.Invoke();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _snapshot?.Dispose();
        base.Dispose(disposing);
    }
}
