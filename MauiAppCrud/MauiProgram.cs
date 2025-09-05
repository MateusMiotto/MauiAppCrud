using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using Windows.UI.Notifications;

namespace MauiAppCrud
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<ClienteRepository>();
            builder.Services.AddSingleton<ModalErrorHandler>();


            builder.Services.AddTransientWithShellRoute<ClienteListPage, ClienteListViewModel>("clientes");
            builder.Services.AddTransientWithShellRoute<ClienteDetailPage, ClienteDetailViewModel>("cliente");

            return builder.Build();
        }
    }
}
