using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebAPI.Interface;
using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.Implementation
{
    public class RolesService : IRolesService
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }

        public RolesService(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DatabaseConnection");
        }

        public Roles Add(Roles rol)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO Roles (Nombre) OUTPUT INSERTED.RolId VALUES (@Nombre)", connection);
                command.Parameters.AddWithValue("@Nombre", rol.Nombre);
                connection.Open();
                rol.RolId = (int)command.ExecuteScalar();
            }
            return rol;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM Roles WHERE RolId = @RolId", connection);
                command.Parameters.AddWithValue("@RolId", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Roles> GetAll()
        {
            var roles = new List<Roles>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Roles", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Roles
                        {
                            RolId = (int)reader["RolId"],
                            Nombre = reader["Nombre"].ToString()
                        });
                    }
                }
            }
            return roles;
        }

        public Roles GetById(int id)
        {
            Roles rol = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Roles WHERE RolId = @RolId", connection);
                command.Parameters.AddWithValue("@RolId", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rol = new Roles
                        {
                            RolId = (int)reader["RolId"],
                            Nombre = reader["Nombre"].ToString()
                        };
                    }
                }
            }
            return rol;
        }

        public void Update(Roles rol)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("UPDATE Roles SET Nombre = @Nombre WHERE RolId = @RolId", connection);
                command.Parameters.AddWithValue("@RolId", rol.RolId);
                command.Parameters.AddWithValue("@Nombre", rol.Nombre);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
