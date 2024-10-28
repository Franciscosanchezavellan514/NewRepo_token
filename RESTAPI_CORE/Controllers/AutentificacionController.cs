//agregar refencias
using RESTAPI_CORE.Modelos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace RESTAPI_CORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutentificacionController : ControllerBase
    {
        private readonly string secretkey;

        public AutentificacionController(IConfiguration config) {
            secretkey = config.GetSection("settings").GetSection("secretkey").ToString();

        }


        //crear metodo para auntentificar al usuario
        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] Usuario request)
        {
            //aqui podemos validar con una base de datos
            if( request.Correo == "admin@gmail.com" && request.Clave == "1234")
            {
                var KeyBytes = Encoding.ASCII.GetBytes(secretkey);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Correo));

                //creamos el token
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    //agregar el tiempo del token
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(KeyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                //lectura del token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                //obtener token creado
                string tokenCreado = tokenHandler.WriteToken(tokenConfig);
                return StatusCode(StatusCodes.Status200OK, new {token = tokenCreado});

            }
            else
            {
                //en el caso de que el usuario no este registrado mande 401
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }

        }
    }
}
