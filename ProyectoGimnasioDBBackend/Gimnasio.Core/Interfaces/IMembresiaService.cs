using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.QueryFilters;
using System.Collections.Generic;
    using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
   public interface IMembresiaService
    {
        Task<ResponseData> GetAllMembresiasAsync(MembresiaQueryFilter membresiaQueryFilter);
        Task<IEnumerable<Membresia>> GetAllClaseDapperAsync();
        Task<Membresia> GetMembresiaByIdAsync(int id);
        Task InsertMembresia(Membresia membresia);
        Task UpdateMembresiaAsync(Membresia membresia);
        Task DeleteMembresiaAsync(Membresia membresia);

        Task<IEnumerable<MembresiaOrdenPrecioResponse>> GetMembresiaOrdenPrecio();
    }
}