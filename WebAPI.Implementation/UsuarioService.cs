using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Interface;
using WebAPI.Model;

namespace WebAPI.Implementation
{
    public class UsuarioService :IUsuarioService
    {
        private readonly string _ConnectionString;
        private readonly IConfiguration _configuration;

        public UsuarioService(IConfiguration configuration)
        {

            _configuration = configuration;

            _ConnectionString = _configuration.GetConnectionString("DatabaseConnection");

        }

        private string CreatePasswordHash(string password, out byte[] salt)
        {
            using var hmac = new HMACSHA256();
            salt = hmac.Key;
            var combinedBytes = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
            var hash = hmac.ComputeHash(combinedBytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<UsuarioEntities> Registrar(UsuarioEntities usuario, string password)
        {
            //se usa task por que se le esta diciendo que no importa cuanto tiempo le tome
            //en ejecutar el procedimiento o peticion, lo tiene que relizar obligatoriamente
            byte[] salt;
            usuario.Password = CreatePasswordHash(password, out salt);

            using (var connection = new SqlConnection(_ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO CatUsuario (UsuarioUsername, UsuarioPassword, UsuarioRol, UsuarioSalt) OUTPUT INSERTED.UsuarioId VALUES (@Username, @PasswordHash, @Role, @Salt)", connection);
                command.Parameters.AddWithValue("@Username", usuario.Username);
                command.Parameters.AddWithValue("@PasswordHash", usuario.Password);
                command.Parameters.AddWithValue("@Salt", salt);
                command.Parameters.AddWithValue("@Role", usuario.Rol);

                await connection.OpenAsync();
                usuario.Id = (int)await command.ExecuteScalarAsync();
            }
            return usuario;
        }


        public IEnumerable<UsuarioEntities> GetAll()
        {
            throw new NotImplementedException();
        }

       public UsuarioEntities GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
