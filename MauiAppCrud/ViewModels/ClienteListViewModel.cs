using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppCrud.Models;

namespace MauiAppCrud.ViewModels
{
    public partial class ClienteListViewModel : ObservableObject
    {
        private readonly ClienteRepository _clienteRepository;

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
            => Shell.Current.GoToAsync($"cliente?id={cliente.ID}");

        [RelayCommand]
        async Task AddCliente()
        {
            await Shell.Current.GoToAsync($"cliente");
        }
    }
}