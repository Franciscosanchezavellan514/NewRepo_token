using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.Interface
{
    public interface ICategoriaService
    {
        Categoria Add(Categoria categoria);
        List<Categoria> GetAll();
        Categoria GetById(int id);
        void Update(Categoria categoria);
        void Delete(int id);
    }
}
