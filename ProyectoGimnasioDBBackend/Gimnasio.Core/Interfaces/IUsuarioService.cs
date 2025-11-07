using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.QueryFilters;
using System.Collections.Generic;
    using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
    public interface IUsuarioService
    {
        Task<ResponseData> GetAllUsuariosAsync(UsuarioQueryFilter usuarioQueryFilter);
        Task<Usuario> GetUsuarioByIdAsync(int id);
        Task InsertUsuario(Usuario usuario);
        Task UpdateUsuarioAsync(Usuario usuario);
        Task DeleteUsuarioAsync(Usuario usuario);
        Task<IEnumerable<Usuario>> GetAllUsuariosDapperAsync();
        Task<IEnumerable<UsuarioAsistenciaResponse>> GetAsistenciasUsuarios();
        
    }
}