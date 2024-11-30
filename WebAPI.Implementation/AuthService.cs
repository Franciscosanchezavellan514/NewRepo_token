using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Interface;
using WebAPI.Model;

namespace WebAPI.Implementation
{
    public class AuthService: IAuthService
    {
        private readonly string _ConnectionString;
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {

            _configuration = configuration;

            _ConnectionString = _configuration.GetConnectionString("DatabaseConnection");

        }

        public async Task<UsuarioEntities> Autenticar(string username, string password)
        {
            UsuarioEntities usuario = null;
            byte[] storedSalt = null;


            using (var connection = new SqlConnection(_ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM CatUsuario WHERE UsuarioUsername = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);

                await connection.OpenAsync();


                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        var passwordHash = reader["UsuarioPassword"].ToString();
                        storedSalt = (byte[])reader["UsuarioSalt"];
                        if (VerifyPasswordHash(password, passwordHash, storedSalt))
                        {
                            usuario = new UsuarioEntities
                            {
                                Id = (int)reader["UsuarioId"],
                                Username = reader["UsuarioUsername"].ToString(),
                                Password = passwordHash,
                                Rol = reader["UsuarioRol"].ToString()
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        private bool VerifyPasswordHash(string password, string storedHash, byte[] salt)
        {
            using var hmac = new HMACSHA256(salt);
            var combinedBytes = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
            var computedHash = hmac.ComputeHash(combinedBytes);
            return storedHash == Convert.ToBase64String(computedHash);
        }

        public string GenerateJwtToken(UsuarioEntities usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            Console.WriteLine(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
