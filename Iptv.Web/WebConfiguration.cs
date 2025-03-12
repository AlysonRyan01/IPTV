using MudBlazor;

namespace Iptv.Web;

public static class WebConfiguration
{
    public static string BackendUrl{ get; set; } = string.Empty;
    
    public const string HttpClientName = "api";
    
    public static MudTheme theme = new()
    {
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Open Sans", "sans-serif"],
                FontWeight =  "600"
            }
        },
        PaletteLight = new PaletteLight
        {
            Primary = "#7C45AD",
            Secondary = "#FFFFFF",
            Tertiary = "#3483FA",
            Background = "#FFFFFF",
            AppbarBackground = "#FFFFFF",
            AppbarText = Colors.Shades.Black,
            TextPrimary = Colors.Shades.Black,
            PrimaryContrastText = Colors.Shades.Black
        }
    };
}
