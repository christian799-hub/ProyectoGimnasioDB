
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;

namespace Gimnasio.Core.Services
{
    public class ClasesService : IClasesService
    {
        public readonly IClasesRepository _clasesRepository;

        public ClasesService(IClasesRepository clasesRepository)
        {
            _clasesRepository = clasesRepository;
        }

        public async Task<IEnumerable<Clase>> GetAllClasesAsync()
        {
            return await _clasesRepository.GetAllClasesAsync();
        }
        public async Task<Clase> GetClaseByIdAsync(int id)
        {
            return await _clasesRepository.GetClaseByIdAsync(id);
        }

        public async Task InsertClase(Clase clase)
        {
            await _clasesRepository.InsertarClase(clase);
        }

        public async Task UpdateClaseAsync(Clase clase)
        {
            await _clasesRepository.UpdateClase(clase);
        }

        public async Task DeleteClaseAsync(Clase clase)
        {
            await _clasesRepository.DeleteClase(clase);
        }

        public async Task<IEnumerable<Clase>> GetClaseByInstructorAsync(int instructorId)
        {
            return await _clasesRepository.GetClaseByInstructorAsync(instructorId);
        }
    
    }
}