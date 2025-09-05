namespace MauiAppCrud.Pages
{
    public partial class ClienteListPage : ContentPage
    {
        public ClienteListPage(ClienteListPageModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}