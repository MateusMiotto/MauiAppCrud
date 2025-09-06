
using Microsoft.Extensions.DependencyInjection;

namespace MauiAppCrud.Pages
{
    public partial class ClienteDetailPage : ContentPage
    {
        public ClienteDetailPage()
        {
            InitializeComponent();
            var navigation = App.Current.Services.GetRequiredService<INavigationService>();
            navigation.InitializeViewModel(this);
        }
    }
}