
using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.QueryFilters;


namespace Gimnasio.Core.Interfaces
{
    public interface IUsuarioMembresiaService
    {
        Task<ResponseData> GetAllUsuarioMembresiasAsync(UsuarioMembresiaQueryFilter usuarioMembresiaQueryFilter);
        Task<IEnumerable<UsuarioMembresia>> GetAllUsuariosDapperAsync();
        Task<UsuarioMembresia> GetUsuarioMembresiaByIdAsync(int id);
        Task InsertUsuarioMembresia(UsuarioMembresia usuarioMembresia);
        Task UpdateUsuarioMembresiaAsync(UsuarioMembresia usuarioMembresia);
        Task DeleteUsuarioMembresiaAsync(UsuarioMembresia usuarioMembresia);

         // Metodo adicional
        Task<bool> UsuarioTieneMembresiaActivaAsync(int usuarioId);
        Task<UsuarioMembresia> GetMembresiaActivaPorUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<UsuarioYMembresiaResponse>> GetUsuarioYMembresia();
    }
}