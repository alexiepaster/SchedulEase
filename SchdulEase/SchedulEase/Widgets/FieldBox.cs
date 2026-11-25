namespace SchedulEase.Widgets;

/// <summary>Rounded input field (the prototype's .form-input) that hosts a TextBox or a ComboBox.</summary>
public class FieldBox : Control
{
    const int Pad = 2;   // room for the focus ring

    public Control Inner { get; }
    public TextBox Box => Inner as TextBox;
    public ComboBox Combo => Inner as ComboBox;
    public bool PasswordToggle { get; }

    readonly Color _fill;
    bool _focused;
    Rectangle _toggleRect;

    FieldBox(Control inner, Color fill, bool passwordToggle)
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);
        BackColor = Color.Transparent;

        _fill = fill;
        PasswordToggle = passwordToggle;
        Inner = inner;
        inner.BackColor = fill;
        inner.ForeColor = Theme.TextPrimary;
        inner.Font = Theme.F(13.5f, true);
        inner.Enter += (_, _) => { _focused = true; Invalidate(); };
        inner.Leave += (_, _) => { _focused = false; Invalidate(); };
        Controls.Add(inner);
        Height = 44;
    }

    public static FieldBox TextInput(string placeholder = "", bool password = false, string value = "")
    {
        var tb = new TextBox { BorderStyle = BorderStyle.None, PlaceholderText = placeholder, Text = value, UseSystemPasswordChar = password };
        return new FieldBox(tb, Color.White, password);
    }

    public static FieldBox Dropdown()
    {
        var cb = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            DrawMode = DrawMode.OwnerDrawFixed,
            ItemHeight = 22
        };
        cb.DrawItem += (s, e) =>
        {
            if (e.Index < 0) return;
            bool inEdit = (e.State & DrawItemState.ComboBoxEdit) != 0;
            bool selected = !inEdit && (e.State & DrawItemState.Selected) != 0;
            using var back = new SolidBrush(selected ? Theme.MaroonSoft : Color.White);
            e.Graphics.FillRectangle(back, e.Bounds);
            TextRenderer.DrawText(e.Graphics, cb.Items[e.Index].ToString(), Theme.F(13, true), e.Bounds,
                selected ? Theme.MaroonPrimary : Theme.TextPrimary,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        };
        return new FieldBox(cb, Color.White, false);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        int right = PasswordToggle ? 54 : 0;
        _toggleRect = new Rectangle(Width - Pad - 54, 0, 54, Height);
        Inner.SetBounds(Pad + 14, (Height - Inner.Height) / 2, Math.Max(10, Width - 2 * Pad - 28 - right), Inner.Height);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (Width < 10 || Height < 10) return;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (_focused) Gfx.DrawRound(g, Color.FromArgb(46, 135, 31, 42), new RectangleF(1.5f, 1.5f, Width - 3, Height - 3), 16, 3f);

        var box = new RectangleF(Pad + 0.5f, Pad + 0.5f, Width - 2 * Pad - 1, Height - 2 * Pad - 1);
        Gfx.FillRound(g, _fill, box, 14);
        Gfx.DrawRound(g, _focused ? Theme.MaroonPrimary : Theme.BorderLight, box, 14, 1.5f);

        if (PasswordToggle)
        {
            bool hidden = Box != null && Box.UseSystemPasswordChar;
            Gfx.Text(g, hidden ? "Show" : "Hide", Theme.F(11, true), _toggleRect, Theme.MaroonPrimary, Gfx.Center);
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        Cursor = PasswordToggle && _toggleRect.Contains(e.Location) ? Cursors.Hand : Cursors.IBeam;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (PasswordToggle && _toggleRect.Contains(e.Location) && Box != null)
        {
            Box.UseSystemPasswordChar = !Box.UseSystemPasswordChar;
            Invalidate();
        }
        Inner.Focus();
    }
}
