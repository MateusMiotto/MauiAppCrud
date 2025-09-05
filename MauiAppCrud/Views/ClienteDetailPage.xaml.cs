
namespace MauiAppCrud.Pages
{
    public partial class ClienteDetailPage : ContentPage
    {
        public ClienteDetailPage(ClienteDetailViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}