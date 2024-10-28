using WebAPI.Model;

namespace WebAPI.Interface
{
    public interface IDispositivoService
    {
        public DispositivosEntities Add(DispositivosEntities dispositivos);
        public List<DispositivosEntities> GetALL();
        public DispositivosEntities GetByID(int id);
        void Update(DispositivosEntities dispositivos);
        void Delete(int id);

    }
}
 