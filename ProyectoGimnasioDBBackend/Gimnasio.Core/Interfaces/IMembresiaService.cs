    using Gimnasio.Core.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
   public interface IMembresiaService
    {
        Task<IEnumerable<Membresia>> GetAllMembresiasAsync();
        Task<Membresia> GetMembresiaByIdAsync(int id);
        Task InsertMembresia(Membresia membresia);
        Task UpdateMembresiaAsync(Membresia membresia);
        Task DeleteMembresiaAsync(Membresia membresia);
    }
}