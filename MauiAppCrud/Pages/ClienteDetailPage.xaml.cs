using MauiAppCrud.PageModels;

namespace MauiAppCrud.Pages
{
    public partial class ClienteDetailPage : ContentPage
    {
        public ClienteDetailPage(ClienteDetailPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}