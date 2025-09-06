using Microsoft.Extensions.DependencyInjection;

namespace MauiAppCrud.Pages
{
    public partial class ClienteListPage : ContentPage
    {
        public ClienteListPage()
        {
            InitializeComponent();
            //var navigation = App.Current.Services.GetRequiredService<INavigationService>();
            var navigation = Application.Current.Handler.MauiContext.Services.GetService<INavigationService>();
            navigation.InitializeViewModel(this);
        }
    }
}