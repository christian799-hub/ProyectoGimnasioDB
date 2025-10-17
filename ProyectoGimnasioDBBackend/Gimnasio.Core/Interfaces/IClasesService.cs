
    using Gimnasio.Core.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
    public interface IClasesService
    {
        Task<IEnumerable<Clase>> GetAllClasesAsync();
        Task<Clase> GetClaseByIdAsync(int id);
        Task InsertClase(Clase clase);
        Task UpdateClaseAsync(Clase clase);
        Task DeleteClaseAsync(Clase clase);

         // Metodo adicional
        Task<IEnumerable<Clase>> GetClaseByInstructorAsync(int instructorId);
    }
}