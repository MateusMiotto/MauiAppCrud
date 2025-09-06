namespace MauiAppCrud.Services
{
    /// <summary>
    /// Provides navigation capabilities.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Navigate to a route.
        /// </summary>
        /// <param name="route">Route string to navigate to.</param>
        /// <returns>A task representing the navigation.</returns>
        Task NavigateToAsync(string route);
    }
}