using Microsoft.AspNetCore.Mvc;
using WebAPI.Interface;
using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.API.Controllers
{
     [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            var categorias = _categoriaService.GetAll();
            return Ok(categorias);
        }

        [HttpPost]
        public ActionResult<Categoria> Add(Categoria categoria)
        {
            var createdCategoria = _categoriaService.Add(categoria);
            return CreatedAtAction(nameof(GetById), new { id = createdCategoria.CategoriaId }, createdCategoria);
        }

        [HttpGet("{id}")]
        public ActionResult<Categoria> GetById(int id)
        {
            var categoria = _categoriaService.GetById(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Categoria categoria)
        {
            var existingCategoria = _categoriaService.GetById(id);
            if (existingCategoria == null) return NotFound();
            categoria.CategoriaId = id;
            _categoriaService.Update(categoria);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingCategoria = _categoriaService.GetById(id);
            if (existingCategoria == null) return NotFound();
            _categoriaService.Delete(id);
            return NoContent();
        }
    }
}
