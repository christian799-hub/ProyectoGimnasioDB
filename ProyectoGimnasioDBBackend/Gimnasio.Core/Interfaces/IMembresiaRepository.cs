using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
    public interface IMembresiasRepository : IBaseRepository<Membresia>
    {
        Task<IEnumerable<Membresia>> GetAllClaseDapperAsync(int limit = 10);
        Task<IEnumerable<MembresiaOrdenPrecioResponse>> GetMembresiaOrdenPrecio();
    }
}