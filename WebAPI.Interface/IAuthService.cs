using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace WebAPI.Interface
{
    public interface IAuthService
    {
        Task<UsuarioEntities> Autenticar(string username, string password);

        string GenerateJwtToken(UsuarioEntities usuario);
    }
}
