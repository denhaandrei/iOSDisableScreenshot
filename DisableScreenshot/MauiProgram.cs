using DisableScreenshot.Views;
using Microsoft.Extensions.Logging;
using Plugin.Maui.ScreenSecurity;

namespace DisableScreenshot
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            Routing.RegisterRoute($"//{nameof(DScreenshot)}", typeof(DScreenshot));
            Routing.RegisterRoute($"//{nameof(AScreenshot)}", typeof(AScreenshot));



            builder.Services.AddSingleton<IScreenSecurity>(ScreenSecurity.Default);
            return builder.Build();
        }
    }
}
