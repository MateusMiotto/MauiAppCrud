using MauiAppCrud.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace MauiAppCrud.Data
{
    /// <summary>
    /// Repository class for managing clientes in the database.
    /// </summary>
    public class ClienteRepository
    {
        private bool _hasBeenInitialized = false;
        private readonly ILogger _logger;
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClienteRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public ClienteRepository(ILogger<ClienteRepository> logger, string? connectionString = null)
        {
            _logger = logger;
            _connectionString = connectionString ?? Constants.DatabasePath;

        }

        /// <summary>
        /// Initializes the database connection and creates the Cliente table if it does not exist.
        /// </summary>
        private async Task Init()
        {
            if (_hasBeenInitialized)
                return;

            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            try
            {
                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Cliente (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Age INT NOT NULL,
                    Address TEXT NOT NULL
                );";
                await createTableCmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating Client table");
                throw;
            }

            _hasBeenInitialized = true;
        }

        /// <summary>
        /// Retrieves a list of all categories from the database.
        /// </summary>
        /// <returns>A list of <see cref="Cliente"/> objects.</returns>
        public async Task<List<Cliente>> ListAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM Cliente";
            var categories = new List<Cliente>();

            await using var reader = await selectCmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                categories.Add(new Cliente
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Age = reader.GetInt32(3),
                    Address = reader.GetString(4),
                });
            }

            return categories;
        }

        /// <summary>
        /// Retrieves a specific Cliente by its ID.
        /// </summary>
        /// <param name="id">The ID of the Cliente.</param>
        /// <returns>A <see cref="Cliente"/> object if found; otherwise, null.</returns>
        public async Task<Cliente?> GetAsync(int id)
        {
            await Init();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM Cliente WHERE ID = @id";
            selectCmd.Parameters.AddWithValue("@id", id);

            await using var reader = await selectCmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Cliente
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Age = reader.GetInt32(3),
                    Address = reader.GetString(4),
                };
            }

            return null;
        }

        /// <summary>
        /// Saves a cliente to the database. If the cliente ID is 0, a new cliente is created; otherwise, the existing cliente is updated.
        /// </summary>
        /// <param name="item">The cliente to save.</param>
        /// <returns>The ID of the saved cliente.</returns>
        public async Task<int> SaveItemAsync(Cliente item)
        {
            await Init();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var saveCmd = connection.CreateCommand();
            if (item.ID == 0)
            {
                saveCmd.CommandText = @"
                INSERT INTO Cliente (Name, LastName, Age, Address)
                VALUES (@Name, @LastName, @Age, @Address);
                SELECT last_insert_rowid();";
            }
            else
            {
                saveCmd.CommandText = @"
                UPDATE Cliente SET Name = @Name, LastName = @LastName, Age = @Age, Address = @Address
                WHERE ID = @ID";
                saveCmd.Parameters.AddWithValue("@ID", item.ID);
            }

            saveCmd.Parameters.AddWithValue("@Name", item.Name);
            saveCmd.Parameters.AddWithValue("@LastName", item.LastName);
            saveCmd.Parameters.AddWithValue("@Age", item.Age);
            saveCmd.Parameters.AddWithValue("@Address", item.Address);

            var result = await saveCmd.ExecuteScalarAsync();
            if (item.ID == 0)
            {
                item.ID = Convert.ToInt32(result);
            }

            return item.ID;
        }

        /// <summary>
        /// Deletes a cliente from the database.
        /// </summary>
        /// <param name="item">The cliente to delete.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> DeleteItemAsync(Cliente item)
        {
            await Init();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM Cliente WHERE ID = @id";
            deleteCmd.Parameters.AddWithValue("@id", item.ID);

            return await deleteCmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Drops the Cliente table from the database.
        /// </summary>
        public async Task DropTableAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var dropTableCmd = connection.CreateCommand();
            dropTableCmd.CommandText = "DROP TABLE IF EXISTS Cliente";

            await dropTableCmd.ExecuteNonQueryAsync();
            _hasBeenInitialized = false;
        }
    }
}