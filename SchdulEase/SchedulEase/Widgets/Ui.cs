namespace SchedulEase.Widgets;

/// <summary>Tiny factory helpers so pages stay short.</summary>
public static class Ui
{
    public static TextLabel Text(Control parent, string text, int x, int y, int w, int h, float px, Color color,
                                 bool bold = false, ContentAlignment align = ContentAlignment.TopLeft, bool mono = false)
    {
        var l = new TextLabel
        {
            Text = text,
            Bounds = new Rectangle(x, y, w, h),
            Font = mono ? Theme.Mono(px, bold) : Theme.F(px, bold),
            ForeColor = color,
            TextAlign = align
        };
        parent.Controls.Add(l);
        return l;
    }

    public static RoundedButton Btn(Control parent, string text, int x, int y, int w, int h, BtnLook look,
                                    Action onClick, int radius = 14, float px = 13f)
    {
        var b = new RoundedButton { Text = text, Look = look, Radius = radius, TextPx = px, Bounds = new Rectangle(x, y, w, h) };
        if (onClick != null) b.Click += (_, _) => onClick();
        parent.Controls.Add(b);
        return b;
    }

    /// <summary>Places a field so its visible box lines up with x / y / w (the control has 2px of focus-ring room).</summary>
    public static FieldBox Field(Control parent, FieldBox f, int x, int y, int w)
    {
        f.SetBounds(x - 2, y - 2, w + 4, 44);
        parent.Controls.Add(f);
        return f;
    }

    /// <summary>Bold label + text field. Advances y past both.</summary>
    public static FieldBox LabeledField(Control parent, string label, string value, int x, ref int y, int w, string placeholder = "")
    {
        Text(parent, label, x, y, w, 16, 12, Theme.TextPrimary, true);
        var f = FieldBox.TextInput(placeholder, false, value);
        Field(parent, f, x, y + 22, w);
        y += 68;
        return f;
    }

    /// <summary>Section title with a small grey tag on the right. Advances y.</summary>
    public static void Section(Control parent, string title, string tag, ref int y)
    {
        Text(parent, title, Theme.Pad, y, 230, 18, 13, Theme.TextPrimary, true);
        if (!string.IsNullOrEmpty(tag))
        {
            var pill = PillLabel.Create(tag, Theme.BorderLight, Theme.TextSecondary, 10, 8);
            pill.Location = new Point(Theme.PageWidth - Theme.Pad - pill.Width, y - 1);
            parent.Controls.Add(pill);
        }
        y += 28;
    }

    /// <summary>Label on the left, value on the right (used in detail sheets). Advances y.</summary>
    public static void Row(Control parent, string label, string value, int x, ref int y, int w)
    {
        Text(parent, label, x, y + 2, 96, 18, 11.5f, Theme.TextSecondary, true);
        Text(parent, value, x + 100, y, w - 100, 36, 13, Theme.TextPrimary, true);
        y += 40;
    }
}
