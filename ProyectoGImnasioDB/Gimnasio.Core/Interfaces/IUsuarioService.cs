    using Gimnasio.Core.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
        Task<Usuario> GetUsuarioByIdAsync(int id);
        Task InsertUsuario(Usuario usuario);
        Task UpdateUsuarioAsync(Usuario usuario);
        Task DeleteUsuarioAsync(Usuario usuario);
    }
}