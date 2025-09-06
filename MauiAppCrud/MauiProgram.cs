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
                        bool firstWindow = true;
                        w.OnWindowCreated(window =>
                        {
                            window.ExtendsContentIntoTitleBar = false; //If you need to completely hide the minimized maximized close button, you need to set this value to false.
                            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);

                            var appWindow = AppWindow.GetFromWindowId(myWndId);

                            if (firstWindow)
                            {
                                appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                                firstWindow = false;
                            }
                            else
                            {
                                appWindow.SetPresenter(AppWindowPresenterKind.Overlapped);
                            }
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
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            return builder.Build();
        }
    }
}
