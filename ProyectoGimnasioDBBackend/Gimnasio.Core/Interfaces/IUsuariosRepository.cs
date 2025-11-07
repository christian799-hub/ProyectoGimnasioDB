using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
    public interface IUsuariosRepository : IBaseRepository<Usuario>
    {
        // Metodo adicional

        Task<IEnumerable<Usuario>> GetAllUsuariosActivosAsync();
        Task<IEnumerable<Usuario>>GetAllUsuariosDapperAsync(int limit = 10);

        Task<IEnumerable<UsuarioAsistenciaResponse>> GetAsistenciasUsuarios();

    }
}