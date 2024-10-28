using Microsoft.AspNetCore.Mvc;
using WebAPI.Interface;
using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetAll()
        {
            var usuarios = _usuarioService.GetAll();
            if (usuarios == null || usuarios.Count == 0)
            {
                return NotFound();
            }
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public ActionResult<Usuario> GetById(int id)
        {
            var usuario = _usuarioService.GetById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public ActionResult<Usuario> Add(Usuario usuario)
        {
            var createdUsuario = _usuarioService.Add(usuario);
            return CreatedAtAction(nameof(GetById), new { id = createdUsuario.UsuarioId }, createdUsuario);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Usuario usuario)
        {
            var existingUsuario = _usuarioService.GetById(id);
            if (existingUsuario == null)
            {
                return NotFound();
            }
            usuario.UsuarioId = id;
            _usuarioService.Update(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingUsuario = _usuarioService.GetById(id);
            if (existingUsuario == null)
            {
                return NotFound();
            }
            _usuarioService.Delete(id);
            return NoContent();
        }
    }
}
