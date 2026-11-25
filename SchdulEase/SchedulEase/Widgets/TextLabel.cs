namespace SchedulEase.Widgets;

/// <summary>Transparent label that lets mouse clicks pass through to whatever is behind it (e.g. a clickable card).</summary>
public class TextLabel : Label
{
    public TextLabel()
    {
        AutoSize = false;
        BackColor = Color.Transparent;
        UseMnemonic = false;
        AutoEllipsis = true;
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == 0x84) // WM_NCHITTEST -> HTTRANSPARENT
        {
            m.Result = new IntPtr(-1);
            return;
        }
        base.WndProc(ref m);
    }
}
