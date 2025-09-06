using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppCrud.Models;

namespace MauiAppCrud.ViewModels
{
    public partial class ClienteListViewModel : ObservableObject, INavigationViewModel, IQueryAttributable
    {
        private readonly ClienteRepository _clienteRepository;
        public INavigationService Navigation { get; set; }

        [ObservableProperty]
        private List<Cliente> _clientes = [];

        public ClienteListViewModel(ClienteRepository projectRepository)
        {
            _clienteRepository = projectRepository;
        }

        public async Task InitializeAsync(IDictionary<string, object>? parameters)
        {
            Clientes = await _clienteRepository.ListAsync();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("refresh"))
            {
                InitializeAsync(null);
            }
        }

        [RelayCommand]
        Task NavigateToCliente(Cliente cliente)
            => Navigation.NavigateToAsync($"cliente?id={cliente.ID}");


        [RelayCommand]
        async Task AddCliente()
        {
            await Navigation.NavigateToAsync($"cliente");
        }
    }
}