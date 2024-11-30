using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace WebAPI.Interface
{
    public interface IUsuarioService
    {
        Task<UsuarioEntities> Registrar(UsuarioEntities  usuario, string password);

        IEnumerable<UsuarioEntities> GetAll();

        UsuarioEntities GetById(int id);
    }
}
