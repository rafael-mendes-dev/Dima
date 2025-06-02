using MudBlazor;

namespace Dima.Web;

public static class Configuration
{
    public const string HttpClientName = "dima";

    public static string BackendUrl { get; set; } = null!;
    
    public static MudTheme Theme = new()
    {
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Raleway", "sans-serif"]
            }
        },
        PaletteLight = new PaletteLight
        {
            Primary = "#1EFA2D",
            PrimaryContrastText = "#000000",
            Secondary = Colors.LightGreen.Darken3,
            Background = Colors.Gray.Lighten4,
            AppbarBackground = "#1EFA2D",
            AppbarText = Colors.Shades.Black,
            TextPrimary = Colors.Shades.Black,
            DrawerText = Colors.Shades.White,
            DrawerBackground = Colors.Green.Darken4,
        },
        PaletteDark = new PaletteDark
        {
            Primary = Colors.LightGreen.Accent3,
            Secondary = Colors.LightGreen.Darken3,
            AppbarBackground = Colors.LightGreen.Accent3,
            AppbarText = Colors.Shades.Black,
            PrimaryContrastText = "#000000"
        }
    };
}