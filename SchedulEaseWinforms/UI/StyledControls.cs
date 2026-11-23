using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace SchedulEaseWinforms.UI
{
    public class MaroonGradientPanel : Panel
    {
        public MaroonGradientPanel() { DoubleBuffered = true; }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (ClientRectangle.Width <= 0 || ClientRectangle.Height <= 0) return;
            using var brush = new LinearGradientBrush(ClientRectangle, Theme.MaroonTop, Theme.MaroonBottom, 145f);
            brush.InterpolationColors = new ColorBlend(3)
            {
                Colors = new[] { Theme.MaroonTop, Theme.MaroonMid, Theme.MaroonBottom },
                Positions = new[] { 0f, .55f, 1f }
            };
            e.Graphics.FillRectangle(brush, ClientRectangle);
        }
    }

    public class RoundedPanel : Panel
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CornerRadius { get; set; } = 16;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color FillColor { get; set; } = Theme.CardWhite;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BorderColor { get; set; } = Theme.BorderGray;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TopCornersOnly { get; set; }

        public RoundedPanel() { DoubleBuffered = true; BackColor = Color.Transparent; }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Width <= 2 || Height <= 2) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (TopCornersOnly) rect.Height += CornerRadius;
            using var path = Theme.RoundedRect(rect, CornerRadius);
            using var brush = new SolidBrush(FillColor);
            e.Graphics.FillPath(brush, path);
            if (BorderColor != Color.Transparent)
            {
                using var pen = new Pen(BorderColor, 1f);
                e.Graphics.DrawPath(pen, path);
            }
        }
    }

    public class RoundedButton : Button
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CornerRadius { get; set; } = 12;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color NormalBg { get; set; } = Theme.MaroonTop;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color HoverBg { get; set; } = Theme.AccentCrimson;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color OutlineColor { get; set; } = Color.Transparent;

        private bool hovering;
        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Font = Theme.Font(9.5f, FontStyle.Bold);
            Cursor = Cursors.Hand;
            UseVisualStyleBackColor = false;
        }
        protected override void OnMouseEnter(EventArgs e) { hovering = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hovering = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width <= 2 || Height <= 2) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using var path = Theme.RoundedRect(rect, CornerRadius);
            using var brush = new SolidBrush(hovering ? HoverBg : NormalBg);
            e.Graphics.FillPath(brush, path);
            if (OutlineColor != Color.Transparent)
            {
                using var pen = new Pen(OutlineColor, 1.2f);
                e.Graphics.DrawPath(pen, path);
            }
            TextRenderer.DrawText(e.Graphics, Text, Font, rect, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
