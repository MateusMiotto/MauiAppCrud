using MauiAppCrud.Views.Base;

namespace MauiAppCrud.Pages
{
    public partial class ClienteListPage : ContentPage, IMauiView
    {
        public ClienteListPage()
        {
            InitializeComponent();
            this.InjectViewModel();
        }
    }
}