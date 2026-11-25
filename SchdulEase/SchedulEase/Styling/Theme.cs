namespace SchedulEase.Styling;

/// <summary>Colors, fonts and layout constants copied from the SchedulEase prototype (CSS :root).</summary>
public static class Theme
{
    static Color H(string hex) => ColorTranslator.FromHtml(hex);

    // Academic maroon
    public static readonly Color MaroonDeep = H("#470C12");
    public static readonly Color MaroonDark = H("#63141C");
    public static readonly Color MaroonPrimary = H("#871F2A");
    public static readonly Color MaroonAccent = H("#A52A38");
    public static readonly Color MaroonSoft = H("#FDF2F3");
    public static readonly Color MaroonBorder = H("#F5D0D4");
    public static readonly Color Rose100 = H("#FFE4E6");
    public static readonly Color Rose200 = H("#FECDD3");

    // Surfaces & text
    public static readonly Color Canvas = H("#F8F9FA");
    public static readonly Color TextPrimary = H("#1E293B");
    public static readonly Color TextSecondary = H("#64748B");
    public static readonly Color TextMuted = H("#94A3B8");
    public static readonly Color BorderLight = H("#E2E8F0");
    public static readonly Color TileGray = H("#F1F5F9");
    public static readonly Color Danger = H("#B91C1C");

    // Status colors
    public static readonly Color AvailText = H("#15803D");
    public static readonly Color AvailBg = H("#DCFCE7");
    public static readonly Color OccText = H("#B45309");
    public static readonly Color OccBg = H("#FEF3C7");
    public static readonly Color ResText = H("#1D4ED8");
    public static readonly Color ResBg = H("#DBEAFE");

    // Layout (the window is a fixed 390px "phone")
    public const int PageWidth = 390;
    public const int Pad = 18;
    public const int CardW = PageWidth - Pad * 2; // 354

    public static (Color Bg, Color Fg) StatusColors(RoomStatus s) => s switch
    {
        RoomStatus.Available => (AvailBg, AvailText),
        RoomStatus.Occupied => (OccBg, OccText),
        _ => (ResBg, ResText)
    };

    static readonly Dictionary<string, Font> Cache = new();

    /// <summary>Segoe UI font measured in pixels (same units as the CSS).</summary>
    public static Font F(float px, bool bold = false)
    {
        string key = $"{px}-{bold}";
        if (!Cache.TryGetValue(key, out var f))
        {
            f = new Font("Segoe UI", px, bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel);
            Cache[key] = f;
        }
        return f;
    }

    public static Font Mono(float px, bool bold = false)
    {
        string key = $"mono-{px}-{bold}";
        if (!Cache.TryGetValue(key, out var f))
        {
            f = new Font("Consolas", px, bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel);
            Cache[key] = f;
        }
        return f;
    }
}
