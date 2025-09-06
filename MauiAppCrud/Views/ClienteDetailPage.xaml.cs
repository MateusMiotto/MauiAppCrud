
using MauiAppCrud.Views.Base;

namespace MauiAppCrud.Pages
{
    public partial class ClienteDetailPage : ContentPage, IMauiView
    {
        private ClienteDetailViewModel ViewModel => (ClienteDetailViewModel)BindingContext; //se precisar chamar viewmodel pelo code-behind 
        public ClienteDetailPage()
        {
            InitializeComponent();
        }
    }
}