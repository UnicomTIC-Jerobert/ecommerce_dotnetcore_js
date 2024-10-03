using Microsoft.Data.Sqlite;
using ProductAPI.Models;

namespace ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqliteCommand("SELECT * FROM Products", connection);
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        ProductId = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Description = reader.GetString(3),
                        ImageURL = reader.GetString(4)
                    });
                }
            }
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            Product product = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqliteCommand("SELECT * FROM Products WHERE ProductId = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    product = new Product
                    {
                        ProductId = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Description = reader.GetString(3),
                        ImageURL = reader.GetString(4)
                    };
                }
            }
            return product;
        }

        public async Task<int> AddProductAsync(Product product)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqliteCommand(
                    "INSERT INTO Products (ProductName, Price, Description, ImageURL) VALUES (@name, @price, @desc, @image)", connection);
                command.Parameters.AddWithValue("@name", product.ProductName);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@desc", product.Description);
                command.Parameters.AddWithValue("@image", product.ImageURL);
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqliteCommand(
                    "UPDATE Products SET ProductName = @name, Price = @price, Description = @desc, ImageURL = @image WHERE ProductId = @id", connection);
                command.Parameters.AddWithValue("@id", product.ProductId);
                command.Parameters.AddWithValue("@name", product.ProductName);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@desc", product.Description);
                command.Parameters.AddWithValue("@image", product.ImageURL);
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqliteCommand("DELETE FROM Products WHERE ProductId = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
