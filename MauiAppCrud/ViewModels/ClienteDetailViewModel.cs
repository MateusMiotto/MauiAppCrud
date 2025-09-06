using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppCrud.Models;
using Microsoft.Maui.Controls;


namespace MauiAppCrud.ViewModels
{
    public partial class ClienteDetailViewModel : ObservableObject, IQueryAttributable, INavigationViewModel
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly ModalErrorHandler _errorHandler;
        public INavigationService Navigation { get; set; }

        private bool _canDelete;
        public const string ClienteQueryKey = "cliente";
        public bool CanSave =>
            !NameHasError &&
            !LastNameHasError &&
            !AgeHasError &&
            !AddressHasError;

        [ObservableProperty] private string _title;
        [ObservableProperty] private string _name;
        [ObservableProperty] private string _lastName;
        [ObservableProperty] private string _address;
        [ObservableProperty] private string _age;

        private Cliente? _cliente;
        private IDictionary<string, object> _queryParameters = new Dictionary<string, object>();

        public bool NameHasError => string.IsNullOrWhiteSpace(Name);
        public bool LastNameHasError => string.IsNullOrWhiteSpace(LastName);
        public bool AgeHasError => string.IsNullOrWhiteSpace(Age) || !int.TryParse(Age, out var age) || age <= 0;
        public bool AddressHasError => string.IsNullOrWhiteSpace(Address);
        partial void OnNameChanged(string value)
        {
            OnPropertyChanged(nameof(NameHasError));
            OnPropertyChanged(nameof(CanSave));
            SaveCommand.NotifyCanExecuteChanged();
        }
        partial void OnLastNameChanged(string value)
        {
            OnPropertyChanged(nameof(LastNameHasError));
            OnPropertyChanged(nameof(CanSave));
            SaveCommand.NotifyCanExecuteChanged();
        }
        partial void OnAgeChanged(string value)
        {
            OnPropertyChanged(nameof(AgeHasError));
            OnPropertyChanged(nameof(CanSave));
            SaveCommand.NotifyCanExecuteChanged();
        }
        partial void OnAddressChanged(string value)
        {
            OnPropertyChanged(nameof(AddressHasError));
            OnPropertyChanged(nameof(CanSave));
            SaveCommand.NotifyCanExecuteChanged();
        }

        public ClienteDetailViewModel(ClienteRepository clienteRepository, ModalErrorHandler errorHandler)
        {
            _clienteRepository = clienteRepository;
            _errorHandler = errorHandler;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _queryParameters = query;
        }

        public async Task InitializeAsync(IDictionary<string, object>? parameters)
        {
            await LoadClienteAsync(parameters ?? _queryParameters);
        }

        private async Task LoadClienteAsync(IDictionary<string, object> query)
        {
            if (query.TryGetValue("id", out var idObj) && int.TryParse(idObj?.ToString(), out int id) && id > 0)
            {
                _cliente = await _clienteRepository.GetAsync(id);

                if (_cliente is null)
                {
                    _errorHandler.HandleError(new Exception($"Cliente Id {id} não é válido."));
                    return;
                }

                Name = _cliente.Name;
                LastName = _cliente.LastName;
                Age = _cliente.Age > 0 ? _cliente.Age.ToString() : string.Empty;
                Address = _cliente.Address;
                Title = "Editar Cliente";
                CanDelete = true;
            }
            else
            {
                _cliente = new Cliente();
                Title = "Novo Cliente";
                CanDelete = false;
            }
        }

        public bool CanDelete
        {
            get => _canDelete;
            set
            {
                if (_canDelete != value)
                {
                    _canDelete = value;
                    OnPropertyChanged(nameof(CanDelete));
                    DeleteCommand.NotifyCanExecuteChanged();
                }
            }
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                _errorHandler.HandleError(new Exception("O campo Nome não pode ser vazio."));
                return;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                _errorHandler.HandleError(new Exception("O campo Sobrenome não pode ser vazio."));
                return;
            }

            if (string.IsNullOrWhiteSpace(Age) || !int.TryParse(Age, out var age) || age <= 0)
            {
                _errorHandler.HandleError(new Exception("A idade deve ser um número inteiro maior que zero."));
                return;
            }
            _cliente.Age = age;

            if (string.IsNullOrWhiteSpace(Address))
            {
                _errorHandler.HandleError(new Exception("O campo Endereço não pode ser vazio."));
                return;
            }

            if (_cliente is null)
            {
                _errorHandler.HandleError(new Exception("Cliente é nulo. Não foi possível salvar."));
                return;
            }

            _cliente.Name = Name;
            _cliente.LastName = LastName;
            _cliente.Age = age;
            _cliente.Address = Address;

            await _clienteRepository.SaveItemAsync(_cliente);

            //await Shell.Current.GoToAsync("..?refresh=true");
            await Navigation.NavigateToAsync("..?refresh=true");
            await AppShell.DisplayToastAsync("Cliente salvo");
        }

        [RelayCommand(CanExecute = nameof(CanDelete))]
        private async Task Delete()
        {
            if (_cliente is null)
            {
                _errorHandler.HandleError(
                    new Exception("Cliente é nulo. Não foi possível deletar."));
                return;
            }
            var page = Application.Current?.Windows.LastOrDefault()?.Page;
            if (page is null)
            {
                _errorHandler.HandleError(new Exception("Página atual não encontrada."));
                return;
            }

            bool confirmDelete = await page.DisplayAlert(
                "Confirmação",
                "Tem certeza que deseja excluir este cliente?",
                "Sim",
                "Não");

            if (!confirmDelete)
                return;

            if (_cliente.ID > 0)
                await _clienteRepository.DeleteItemAsync(_cliente);

            await Navigation.NavigateToAsync("..?refresh=true");
            //await Shell.Current.GoToAsync("..?refresh=true");

            await AppShell.DisplayToastAsync("Cliente deletado");
        }
    }
}