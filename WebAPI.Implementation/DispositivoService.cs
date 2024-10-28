using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebAPI.Interface;
using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.Implementation
{
    public class DispositivoService : IDispositivoService
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }

        public DispositivoService(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DatabaseConnection");
        }

        public DispositivosEntities Add(DispositivosEntities dispositivos)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO CatDispositivo (DispositivoNombre) OUTPUT INSERTED.DispositivoId VALUES (@DispositivoNombre)", connection);
                command.Parameters.AddWithValue("@DispositivoNombre", dispositivos.DispositivoNombre);
                connection.Open();
                dispositivos.DispositivoId = (int)command.ExecuteScalar(); // Obtener el ID del nuevo registro
            }
            return dispositivos;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM CatDispositivo WHERE DispositivoId = @DispositivoId", connection);
                command.Parameters.AddWithValue("@DispositivoId", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<DispositivosEntities> GetALL()
        {
            var dispositivos = new List<DispositivosEntities>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM CatDispositivo", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dispositivos.Add(new DispositivosEntities
                        {
                            DispositivoId = (int)reader["DispositivoId"],
                            DispositivoNombre = reader["DispositivoNombre"].ToString(),
                            DispositivoState = (bool)reader["DispositivoState"]
                        });
                    }
                }
            }
            return dispositivos;
        }

        public DispositivosEntities GetByID(int id)
        {
            DispositivosEntities dispositivo = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM CatDispositivo WHERE DispositivoId = @DispositivoId", connection);
                command.Parameters.AddWithValue("@DispositivoId", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dispositivo = new DispositivosEntities
                        {
                            DispositivoId = (int)reader["DispositivoId"],
                            DispositivoNombre = reader["DispositivoNombre"].ToString(),
                            DispositivoState = (bool)reader["DispositivoState"]
                        };
                    }
                }
            }
            return dispositivo;
        }

        public void Update(DispositivosEntities dispositivos)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("UPDATE CatDispositivo SET DispositivoNombre = @DispositivoNombre, DispositivoState = @DispositivoState WHERE DispositivoId = @DispositivoId", connection);
                command.Parameters.AddWithValue("@DispositivoId", dispositivos.DispositivoId);
                command.Parameters.AddWithValue("@DispositivoNombre", dispositivos.DispositivoNombre);
                command.Parameters.AddWithValue("@DispositivoState", dispositivos.DispositivoState);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
