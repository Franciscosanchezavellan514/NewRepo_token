using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.Interface
{
    public interface IUsuarioService
    {
        Usuario Add(Usuario usuario);
        List<Usuario> GetAll();
        Usuario GetById(int id);
        void Update(Usuario usuario);
        void Delete(int id);
    }
}
