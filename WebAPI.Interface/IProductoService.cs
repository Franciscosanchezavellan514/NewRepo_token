using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.Interface
{
    public interface IProductoService
    {
        Producto Add(Producto producto);
        List<Producto> GetAll();
        Producto GetById(int id);
        void Update(Producto producto);
        void Delete(int id);
    }
}
