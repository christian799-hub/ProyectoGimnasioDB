
using Gimnasio.Core.Entities;


namespace Gimnasio.Core.Interfaces
{
    public interface IUsuarioMembresiaService
    {
        Task<IEnumerable<UsuarioMembresia>> GetAllUsuarioMembresiasAsync();
        Task<UsuarioMembresia> GetUsuarioMembresiaByIdAsync(int id);
        Task InsertUsuarioMembresia(UsuarioMembresia usuarioMembresia);
        Task UpdateUsuarioMembresiaAsync(UsuarioMembresia usuarioMembresia);
        Task DeleteUsuarioMembresiaAsync(UsuarioMembresia usuarioMembresia);
        Task<bool> UsuarioTieneMembresiaActivaAsync(int usuarioId);
        Task<UsuarioMembresia> GetMembresiaActivaPorUsuarioIdAsync(int usuarioId);
    }
}