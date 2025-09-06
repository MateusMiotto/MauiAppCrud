namespace MauiAppCrud.ViewModels.Base
{
    /// <summary>
    /// Provides navigation service access for view models.
    /// </summary>
    public interface INavigationViewModel
    {
        /// <summary>
        /// Navigation service.
        /// </summary>
        INavigationService Navigation { get; set; }
    }
}