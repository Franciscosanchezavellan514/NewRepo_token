using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Implementation;
using WebAPI.Interface;
using WebAPI.Model;

namespace WebAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;



        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("Autenticar")]
        public async Task<IActionResult> Authenticate([FromBody] UsuarioDto Usuario)
        {
            var user = await _authService.Autenticar(Usuario.Username, Usuario.Password);

            if (user == null)
                return BadRequest(new { message = "Datos Incorrectos" });


            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

    }
}
