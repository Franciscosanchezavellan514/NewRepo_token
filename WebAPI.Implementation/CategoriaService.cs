using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebAPI.Interface;
using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.Implementation
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }

        public CategoriaService(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DatabaseConnection");
        }

        public Categoria Add(Categoria categoria)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO Categoria (Nombre) OUTPUT INSERTED.CategoriaId VALUES (@Nombre)", connection);
                command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                connection.Open();
                categoria.CategoriaId = (int)command.ExecuteScalar();
            }
            return categoria;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM Categoria WHERE CategoriaId = @CategoriaId", connection);
                command.Parameters.AddWithValue("@CategoriaId", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Categoria> GetAll()
        {
            var categorias = new List<Categoria>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Categoria", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categorias.Add(new Categoria
                        {
                            CategoriaId = (int)reader["CategoriaId"],
                            Nombre = reader["Nombre"].ToString()
                        });
                    }
                }
            }
            return categorias;
        }

        public Categoria GetById(int id)
        {
            Categoria categoria = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Categoria WHERE CategoriaId = @CategoriaId", connection);
                command.Parameters.AddWithValue("@CategoriaId", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        categoria = new Categoria
                        {
                            CategoriaId = (int)reader["CategoriaId"],
                            Nombre = reader["Nombre"].ToString()
                        };
                    }
                }
            }
            return categoria;
        }

        public void Update(Categoria categoria)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("UPDATE Categoria SET Nombre = @Nombre WHERE CategoriaId = @CategoriaId", connection);
                command.Parameters.AddWithValue("@CategoriaId", categoria.CategoriaId);
                command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
