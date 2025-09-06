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
                            window.ExtendsContentIntoTitleBar = false;

                            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
                            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

                            DisplayArea area = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);

                            if (firstWindow)
                            {
                                firstWindow = false;
                                switch (appWindow.Presenter)
                                {
                                    case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                                        overlappedPresenter.SetBorderAndTitleBar(true, true); //to set first windown FULLSCRENN with no title border and windows buttons set this to false
                                        overlappedPresenter.Maximize();
                                        break;
                                }
                            }
                            else
                            {
                                appWindow.SetPresenter(AppWindowPresenterKind.Overlapped);

                                // Centraliza as demais janelas
                                int x = (area.WorkArea.Width - appWindow.Size.Width) / 2;
                                int y = (area.WorkArea.Height - appWindow.Size.Height) / 2;
                                appWindow.Move(new Windows.Graphics.PointInt32(
                                    x + area.WorkArea.X,
                                    y + area.WorkArea.Y));
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
