using Gimnasio.Core.Entities;

namespace  Gimnasio.Core.Interfaces
{
    public interface IUsuarioMembresiasRepository
    {
        //CRUD
        Task<IEnumerable<UsuarioMembresia>> GetAllUsuarioMembresiasAsync();
        Task<UsuarioMembresia> GetUsuarioMembresiaByIdAsync(int id);
        Task InsertarUsuarioMembresia(UsuarioMembresia usuarioMembresia);
        Task UpdateUsuarioMembresia(UsuarioMembresia usuarioMembresia);
        Task DeleteUsuarioMembresia(UsuarioMembresia usuarioMembresia);

        // Metodos adicionales
        Task<bool> UsuarioTieneMembresiaActivaAsync(int usuarioId);
        Task<UsuarioMembresia> GetMembresiaActivaByUsuarioAsync(int usuarioId);
        Task<UsuarioMembresia> GetMembresiaActivaByUsuarioAndFechaAsync(int usuarioId, DateOnly fecha);
    }
}
