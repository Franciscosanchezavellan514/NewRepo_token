using Microsoft.AspNetCore.Mvc;
using WebAPI.Interface;
using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.API.Controllers
{
     [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Producto>> GetAll()
        {
            var productos = _productoService.GetAll();
            if (productos == null || productos.Count == 0)
            {
                return NotFound();
            }
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public ActionResult<Producto> GetById(int id)
        {
            var producto = _productoService.GetById(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPost]
        public ActionResult<Producto> Add(Producto producto)
        {
            var createdProducto = _productoService.Add(producto);
            return CreatedAtAction(nameof(GetById), new { id = createdProducto.ProductoId }, createdProducto);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Producto producto)
        {
            var existingProducto = _productoService.GetById(id);
            if (existingProducto == null)
            {
                return NotFound();
            }
            producto.ProductoId = id;
            _productoService.Update(producto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingProducto = _productoService.GetById(id);
            if (existingProducto == null)
            {
                return NotFound();
            }
            _productoService.Delete(id);
            return NoContent();
        }
    }
}
