using Microsoft.AspNetCore.Mvc;
using WebAPI.Interface;
using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Roles>> GetAll()
        {
            var roles = _rolesService.GetAll();
            if (roles == null || roles.Count == 0)
            {
                return NotFound();
            }
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public ActionResult<Roles> GetById(int id)
        {
            var rol = _rolesService.GetById(id);
            if (rol == null)
            {
                return NotFound();
            }
            return Ok(rol);
        }

        [HttpPost]
        public ActionResult<Roles> Add(Roles rol)
        {
            var createdRol = _rolesService.Add(rol);
            return CreatedAtAction(nameof(GetById), new { id = createdRol.RolId }, createdRol);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Roles rol)
        {
            var existingRol = _rolesService.GetById(id);
            if (existingRol == null)
            {
                return NotFound();
            }
            rol.RolId = id;
            _rolesService.Update(rol);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingRol = _rolesService.GetById(id);
            if (existingRol == null)
            {
                return NotFound();
            }
            _rolesService.Delete(id);
            return NoContent();
        }
    }
}
