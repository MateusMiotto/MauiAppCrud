using MauiAppCrud.ViewModels;

namespace MauiAppCrud.Pages
{
    public partial class ClienteListPage : ContentPage
    {
        public ClienteListPage(ClienteListViewModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}