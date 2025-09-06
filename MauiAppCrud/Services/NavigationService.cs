using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Linq;
using MauiAppCrud.Views.Base;

namespace MauiAppCrud.Services
{
    /// <summary>
    /// Default navigation service.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<string, Type> _routes = new Dictionary<string, Type>
        {
            ["clientes"] = typeof(ClienteListPage),
            ["cliente"] = typeof(ClienteDetailPage)
        };

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public async Task NavigateToAsync(string route)
        {
            if (route.StartsWith(".."))
            {
                if (OperatingSystem.IsWindows())
                {
                    var window = Application.Current?.Windows.LastOrDefault();
                    if (window != null && Application.Current?.Windows.Count > 1)
                        Application.Current?.CloseWindow(window);
                }
                await Shell.Current.GoToAsync(route);
                return;
            }

            var baseRoute = route.Split('?')[0];
            if (!_routes.TryGetValue(baseRoute, out var pageType))
            {
                await Shell.Current.GoToAsync(route);
                return;
            }

            var page = Activator.CreateInstance(pageType) as Page;
            if (page is null)
                return;

            InitializeViewModel(page);

            var query = route.Contains('?') ? ParseQuery(route) : new Dictionary<string, object>();

            if (page.BindingContext is IQueryAttributable queryable)
                queryable.ApplyQueryAttributes(query);

            if (page.BindingContext is INavigationViewModel navigationViewModel)
                await navigationViewModel.InitializeAsync(query);

            if (OperatingSystem.IsWindows())
            {
                var window = new Window(page);
                window.SetBinding(Window.TitleProperty, new Binding(nameof(Page.Title), source: page));
                Application.Current?.OpenWindow(window);
                return;
            }

            await Shell.Current.Navigation.PushAsync(page);
        }

        /// <inheritdoc />
        public void InitializeViewModel(Page page)
        {
            if (page.BindingContext is not null)
                return;

            if (page is IMauiView mauiView)
                mauiView.InjectViewModel(_serviceProvider);
        }

        private static IDictionary<string, object> ParseQuery(string route)
        {
            var result = new Dictionary<string, object>();
            var query = route.Split('?', 2).Length > 1 ? route.Split('?', 2)[1] : string.Empty;
            foreach (var part in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var kv = part.Split('=', 2);
                if (kv.Length == 2)
                    result[kv[0]] = Uri.UnescapeDataString(kv[1]);
            }
            return result;
        }
    }
}
