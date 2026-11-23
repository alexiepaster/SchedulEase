using System.Drawing;
using System.Drawing.Drawing2D;

namespace SchedulEaseWinforms.UI
{
    public static class Theme
    {
        public static readonly Color MaroonTop = ColorTranslator.FromHtml("#871F2A");
        public static readonly Color MaroonMid = ColorTranslator.FromHtml("#63141C");
        public static readonly Color MaroonBottom = ColorTranslator.FromHtml("#470C12");
        public static readonly Color AccentCrimson = ColorTranslator.FromHtml("#9E2A35");
        public static readonly Color BlushTint = ColorTranslator.FromHtml("#FDF2F3");

        public static readonly Color BlushBorder = ColorTranslator.FromHtml("#F3D5D8");

        public static readonly Color SheetBg = ColorTranslator.FromHtml("#F8F9FA");
        public static readonly Color CardWhite = ColorTranslator.FromHtml("#FFFFFF");
        public static readonly Color BorderGray = ColorTranslator.FromHtml("#E2E8F0");

        public static readonly Color TextDark = ColorTranslator.FromHtml("#1E293B");
        public static readonly Color TextMuted = ColorTranslator.FromHtml("#64748B");

        public static readonly Color GreenBg = ColorTranslator.FromHtml("#DCFCE7");
        public static readonly Color GreenText = ColorTranslator.FromHtml("#15803D");
        public static readonly Color AmberBg = ColorTranslator.FromHtml("#FEF3C7");
        public static readonly Color AmberText = ColorTranslator.FromHtml("#B45309");

        public static Font Font(float size, FontStyle style = FontStyle.Regular) =>
            new Font("Segoe UI", size, style, GraphicsUnit.Point);

        public static Font Mono(float size, FontStyle style = FontStyle.Bold) =>
            new Font("Consolas", size, style, GraphicsUnit.Point);

        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}