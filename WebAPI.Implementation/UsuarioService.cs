using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebAPI.Interface;
using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.Implementation
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }

        public UsuarioService(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DatabaseConnection");
        }

        public Usuario Add(Usuario usuario)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO Usuario (Nombre, Correo, RolId) OUTPUT INSERTED.UsuarioId VALUES (@Nombre, @Correo, @RolId)", connection);
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Correo", usuario.Correo);
                command.Parameters.AddWithValue("@RolId", (object)usuario.RolId ?? DBNull.Value);
                connection.Open();
                usuario.UsuarioId = (int)command.ExecuteScalar();
            }
            return usuario;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM Usuario WHERE UsuarioId = @UsuarioId", connection);
                command.Parameters.AddWithValue("@UsuarioId", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Usuario> GetAll()
        {
            var usuarios = new List<Usuario>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Usuario", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            UsuarioId = (int)reader["UsuarioId"],
                            Nombre = reader["Nombre"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            RolId = reader["RolId"] as int?
                        });
                    }
                }
            }
            return usuarios;
        }

        public Usuario GetById(int id)
        {
            Usuario usuario = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Usuario WHERE UsuarioId = @UsuarioId", connection);
                command.Parameters.AddWithValue("@UsuarioId", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            UsuarioId = (int)reader["UsuarioId"],
                            Nombre = reader["Nombre"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            RolId = reader["RolId"] as int?
                        };
                    }
                }
            }
            return usuario;
        }

        public void Update(Usuario usuario)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("UPDATE Usuario SET Nombre = @Nombre, Correo = @Correo, RolId = @RolId WHERE UsuarioId = @UsuarioId", connection);
                command.Parameters.AddWithValue("@UsuarioId", usuario.UsuarioId);
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Correo", usuario.Correo);
                command.Parameters.AddWithValue("@RolId", (object)usuario.RolId ?? DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
