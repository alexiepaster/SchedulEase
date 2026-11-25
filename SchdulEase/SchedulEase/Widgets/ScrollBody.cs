namespace SchedulEase.Widgets;

/// <summary>Scrollable page body with a hidden scrollbar (mouse wheel), like the prototype's .scroll-body.</summary>
public class ScrollBody : Panel, IMessageFilter
{
    public FlatPanel Content { get; } = new FlatPanel();
    int _contentHeight;

    public ScrollBody()
    {
        DoubleBuffered = true;
        BackColor = Theme.Canvas;
        Content.BackColor = Theme.Canvas;
        Content.SetBounds(0, 0, Theme.PageWidth, 100);
        Controls.Add(Content);
    }

    public void SetContentHeight(int h)
    {
        _contentHeight = h;
        Relayout();
    }

    void Relayout()
    {
        Content.Width = Width;
        Content.Height = Math.Max(_contentHeight, Height);
        ScrollBy(0);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        Relayout();
    }

    public void ScrollBy(int delta)
    {
        int min = Math.Min(0, Height - Content.Height);
        Content.Top = Math.Max(min, Math.Min(0, Content.Top + delta));
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        Application.RemoveMessageFilter(this);
        Application.AddMessageFilter(this);
    }

    protected override void Dispose(bool disposing)
    {
        Application.RemoveMessageFilter(this);
        base.Dispose(disposing);
    }

    public bool PreFilterMessage(ref Message m)
    {
        if (m.Msg != 0x020A || !IsHandleCreated || !Visible) return false;      // WM_MOUSEWHEEL
        if (!RectangleToScreen(ClientRectangle).Contains(Cursor.Position)) return false;
        int delta = (short)(((long)m.WParam >> 16) & 0xFFFF);
        ScrollBy(delta / 2);
        return true;
    }
}
