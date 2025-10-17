using Gimnasio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
    public interface IMembresiasRepository
    {
        Task<IEnumerable<Membresia>> GetAllMembresiasAsync();
        Task<Membresia> GetMembresiaByIdAsync(int id);
        Task InsertarMembresia(Membresia membresia);
        Task UpdateMembresia(Membresia membresia);
        Task DeleteMembresia(Membresia membresia);
    }
}