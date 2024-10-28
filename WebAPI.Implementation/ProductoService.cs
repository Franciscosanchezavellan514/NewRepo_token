using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebAPI.Interface;
using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.Implementation
{
    public class ProductoService : IProductoService
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }

        public ProductoService(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DatabaseConnection");
        }

        public Producto Add(Producto producto)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO Producto (Nombre, Precio, CategoriaId) OUTPUT INSERTED.ProductoId VALUES (@Nombre, @Precio, @CategoriaId)", connection);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@CategoriaId", (object)producto.CategoriaId ?? DBNull.Value);
                connection.Open();
                producto.ProductoId = (int)command.ExecuteScalar();
            }
            return producto;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM Producto WHERE ProductoId = @ProductoId", connection);
                command.Parameters.AddWithValue("@ProductoId", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Producto> GetAll()
        {
            var productos = new List<Producto>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Producto", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto
                        {
                            ProductoId = (int)reader["ProductoId"],
                            Nombre = reader["Nombre"].ToString(),
                            Precio = (decimal)reader["Precio"],
                            CategoriaId = reader["CategoriaId"] as int?
                        });
                    }
                }
            }
            return productos;
        }

        public Producto GetById(int id)
        {
            Producto producto = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Producto WHERE ProductoId = @ProductoId", connection);
                command.Parameters.AddWithValue("@ProductoId", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        producto = new Producto
                        {
                            ProductoId = (int)reader["ProductoId"],
                            Nombre = reader["Nombre"].ToString(),
                            Precio = (decimal)reader["Precio"],
                            CategoriaId = reader["CategoriaId"] as int?
                        };
                    }
                }
            }
            return producto;
        }

        public void Update(Producto producto)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("UPDATE Producto SET Nombre = @Nombre, Precio = @Precio, CategoriaId = @CategoriaId WHERE ProductoId = @ProductoId", connection);
                command.Parameters.AddWithValue("@ProductoId", producto.ProductoId);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@CategoriaId", (object)producto.CategoriaId ?? DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
