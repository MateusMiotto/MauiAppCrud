using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppCrud.Data;
using MauiAppCrud.Models;

namespace MauiAppCrud.PageModels
{
    public partial class ClienteListPageModel : ObservableObject
    {
        private readonly ClienteRepository _clienteRepository;

        [ObservableProperty]
        private List<Cliente> _clientes = [];

        public ClienteListPageModel(ClienteRepository projectRepository)
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