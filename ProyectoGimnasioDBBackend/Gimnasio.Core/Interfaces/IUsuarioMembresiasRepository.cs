using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;

namespace  Gimnasio.Core.Interfaces
{
    public interface IUsuarioMembresiasRepository : IBaseRepository<UsuarioMembresia>
    {
        //CRUD
        new Task<IEnumerable<UsuarioMembresia>> GetAllAsync();
        Task<IEnumerable<UsuarioMembresia>> GetAllUsuariosDapperAsync(int limit = 10);
        new Task<UsuarioMembresia> GetByIdAsync(int id);

        // Metodos adicionales
        Task<bool> UsuarioTieneMembresiaActivaAsync(int usuarioId);
        Task<UsuarioMembresia> GetMembresiaActivaByUsuarioAsync(int usuarioId);
        Task<UsuarioMembresia> GetMembresiaActivaByUsuarioAndFechaAsync(int usuarioId, DateOnly fecha);
        Task<IEnumerable<UsuarioYMembresiaResponse>> GetUsuarioYMembresia();
    }
}
