using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui.Storage;
using MauiAppCrud.Data;
using MauiAppCrud.Models;
using Xunit;

namespace MauiAppCrud.Tests.Repositories;

public class ClienteRepositoryTests
{
    //public ClienteRepositoryTests()
    //{

    //}

    [Fact]
    public async Task SaveAndRetrieveCliente()
    {
        var tempDbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.db");
        var repo = new ClienteRepository(
            NullLogger<ClienteRepository>.Instance,
            $"Data Source={tempDbPath}");

        await repo.DropTableAsync();

        var cliente = new Cliente
        {
            Name = "John",
            LastName = "Doe",
            Age = 30,
            Address = "123 Street"
        };

        var id = await repo.SaveItemAsync(cliente);
        Assert.True(id > 0);

        var list = await repo.ListAsync();
        Assert.Single(list);
        Assert.Equal("John", list[0].Name);

        if (File.Exists(tempDbPath))
        {
            File.Delete(tempDbPath);
        }
    }


}
