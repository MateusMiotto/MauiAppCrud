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
                                appWindow.SetPresenter(AppWindowPresenterKind.Overlapped);

                                // Ocupa toda a área disponível
                                appWindow.Resize(new Windows.Graphics.SizeInt32(
                                    area.WorkArea.Width,
                                    area.WorkArea.Height));

                                // Posiciona a janela no canto superior esquerdo da área de trabalho
                                appWindow.Move(new Windows.Graphics.PointInt32(
                                    area.WorkArea.X,
                                    area.WorkArea.Y));

                                firstWindow = false;


                                //abrir em FULLSCREEN
                                //window.ExtendsContentIntoTitleBar = true; //If you need to completely hide the minimized maximized close button, you need to set this value to false.
                                //IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                                //WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
                                //var _appWindow = AppWindow.GetFromWindowId(myWndId);
                                //_appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
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
