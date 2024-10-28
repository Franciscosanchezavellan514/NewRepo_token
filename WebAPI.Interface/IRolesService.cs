using WebAPI.Model;
using System.Collections.Generic;

namespace WebAPI.Interface
{
    public interface IRolesService
    {
        Roles Add(Roles rol);
        List<Roles> GetAll();
        Roles GetById(int id);
        void Update(Roles rol);
        void Delete(int id);
    }
}
