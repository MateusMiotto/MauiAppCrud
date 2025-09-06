namespace MauiAppCrud.Services
{
    /// <summary>
    /// Default navigation service.
    /// </summary>
    public class NavigationService : INavigationService
    {
        /// <inheritdoc />
        public Task NavigateToAsync(string route)
        {
            if (OperatingSystem.IsWindows())
            {
                if (route.StartsWith(".."))
                {
                    var window = Application.Current?.Windows.LastOrDefault();
                    if (window != null && Application.Current?.Windows.Count > 1)
                        Application.Current?.CloseWindow(window);
                    return Shell.Current.GoToAsync(route);
                }

                var page = Routing.GetOrCreateContent(route) as Page;
                if (page is not null)
                    Application.Current?.OpenWindow(new Window(page));
                return Task.CompletedTask;
            }

            return Shell.Current.GoToAsync(route);
        }
    }
}