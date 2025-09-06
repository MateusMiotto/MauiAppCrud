using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Syncfusion.Maui.Toolkit.Hosting;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
#endif
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
                // Add this method for builder to set fullscreen on windows.
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    events.AddWindows(w =>
                    {
                        w.OnWindowCreated(window =>
                        {
                            window.ExtendsContentIntoTitleBar = true; //If you need to completely hide the minimized maximized close button, you need to set this value to false.
                            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
                            var _appWindow = AppWindow.GetFromWindowId(myWndId);
                            _appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                        });
                    });
#endif
                })
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
