using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui.Storage;
using MauiAppCrud.Data;
using MauiAppCrud.Models;
using Xunit;

namespace MauiAppCrud.Tests.Repositories;

public class ClienteRepositoryTests
{
    public ClienteRepositoryTests()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        var fileSystem = new TestFileSystem(tempDir);
        FileSystem.Current = fileSystem;
    }

    [Fact]
    public async Task SaveAndRetrieveCliente()
    {
        var repo = new ClienteRepository(NullLogger<ClienteRepository>.Instance);
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
    }

    private class TestFileSystem : IFileSystem
    {
        private readonly string _dir;
        public TestFileSystem(string dir) => _dir = dir;
        public string CacheDirectory => _dir;
        public string AppDataDirectory => _dir;
    }
}
