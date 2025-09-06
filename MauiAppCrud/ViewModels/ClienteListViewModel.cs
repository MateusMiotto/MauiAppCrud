using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppCrud.Models;

namespace MauiAppCrud.ViewModels
{
    public partial class ClienteListViewModel : ObservableObject, INavigationViewModel
    {
        private readonly ClienteRepository _clienteRepository;
        public INavigationService Navigation { get; set; }

        [ObservableProperty]
        private List<Cliente> _clientes = [];

        public ClienteListViewModel(ClienteRepository projectRepository)
        {
            _clienteRepository = projectRepository;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            Clientes = await _clienteRepository.ListAsync();
        }

        [RelayCommand]
        Task NavigateToCliente(Cliente cliente)
            //=> Shell.Current.GoToAsync($"cliente?id={cliente.ID}");
            => Navigation.NavigateToAsync($"cliente?id={cliente.ID}");


        [RelayCommand]
        async Task AddCliente()
        {
            await Navigation.NavigateToAsync($"cliente");
            //await Shell.Current.GoToAsync($"cliente");
        }
    }
}