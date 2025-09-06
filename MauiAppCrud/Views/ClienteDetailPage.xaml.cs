
using Microsoft.Extensions.DependencyInjection;

namespace MauiAppCrud.Pages
{
    public partial class ClienteDetailPage : ContentPage
    {
        public ClienteDetailPage()
        {
            InitializeComponent();
            //var navigation = App.Current.Services.GetRequiredService<INavigationService>();

            var navigation = Application.Current.Handler.MauiContext.Services.GetService<INavigationService>();
            navigation.InitializeViewModel(this);
        }
    }
}