
using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
    public interface IClasesService
    {
        Task<ResponseData> GetAllClasesAsync(ClasesQueryFilter clasesQueryFilter);
        Task<IEnumerable<Clase>> GetAllClaseDapperAsync(); 
        Task<Clase> GetClaseByIdAsync(int id);
        Task InsertClase(Clase clase);
        Task UpdateClaseAsync(Clase clase);
        Task DeleteClaseAsync(Clase clase);

         // Metodo adicional
        Task<IEnumerable<Clase>> GetClaseByInstructorAsync(int instructorId);
        Task<IEnumerable<ClaseInstructorResponse>> GetClaseInstructor();
    }
}