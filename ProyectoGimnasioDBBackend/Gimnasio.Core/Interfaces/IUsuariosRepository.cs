using Gimnasio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
    public interface IUsuariosRepository
    {
        // CRUD
        
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
        Task<Usuario> GetUsuariosByIdAsync(int id);
        Task InsertarUsuario(Usuario usuario);
        Task UpdateUsuarios(Usuario usuario);
        Task DeleteUsuarios(Usuario usuario);

        // Metodo adicional

        Task<IEnumerable<Usuario>> GetAllUsuariosActivosAsync();
        Task<UsuarioMembresia> GetMembresiaActivaByUsuarioAsyncConFecha(int usuarioId, DateOnly fecha);

    }
}