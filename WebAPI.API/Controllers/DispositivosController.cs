using Microsoft.AspNetCore.Mvc;
using WebAPI.Interface;
using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.API.Controllers
{
     [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DispositivosController : ControllerBase
    {
        private readonly IDispositivoService _dispositivoService;

        public DispositivosController(IDispositivoService dispositivoService)
        {
            _dispositivoService = dispositivoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DispositivosEntities>> GetAll()
        {
            var dispositivos = _dispositivoService.GetALL();
            if (dispositivos == null)
            {
                return NotFound();
            }
            return Ok(dispositivos);
        }

        [HttpPost]
        public ActionResult Add(DispositivosEntities dispositivos)
        {
            _dispositivoService.Add(dispositivos);
            return CreatedAtAction(nameof(GetByID), new { id = dispositivos.DispositivoId }, dispositivos);
        }

        [HttpGet("{id}")]
        public ActionResult<DispositivosEntities> GetByID(int id)
        {
            var dispositivos = _dispositivoService.GetByID(id);
            if (dispositivos == null) return NotFound();
            return Ok(dispositivos);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, DispositivosEntities dispositivos)
        {
            var find = _dispositivoService.GetByID(id);
            if (find == null) return NotFound();
            dispositivos.DispositivoId = id;
            _dispositivoService.Update(dispositivos);
            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var find = _dispositivoService.GetByID(id);
            if (find == null) return NotFound();
            _dispositivoService.Delete(id);
            return NoContent(); // 204 No Content
        }
    }
}
