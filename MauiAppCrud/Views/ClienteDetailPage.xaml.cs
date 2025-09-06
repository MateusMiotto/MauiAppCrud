
using MauiAppCrud.Views.Base;

namespace MauiAppCrud.Pages
{
    public partial class ClienteDetailPage : ContentPage, IMauiView
    {
        public ClienteDetailPage()
        {
            InitializeComponent();
            this.InjectViewModel();
        }
    }
}