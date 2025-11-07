
using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.QueryFilters;

namespace Gimnasio.Core.Services
{
    public class ClasesService : IClasesService
    {
        public readonly IUnitOfWork _unitOfWork;

        public ClasesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> GetAllClasesAsync(
            ClasesQueryFilter clasesQueryFilter
        )
        {
            var clases = await _unitOfWork.claseRepository.GetAllAsync();
            if(clasesQueryFilter.Descripcion != null)
            {
                clases = clases.Where(c => c.Descripcion.Contains(clasesQueryFilter.Descripcion));
            }
            if(clasesQueryFilter.InstructorId != null)
            {
                clases = clases.Where(c => c.InstructorId == clasesQueryFilter.InstructorId);
            }
            if(clasesQueryFilter.CapacidadMaxima != null)
            {
                clases = clases.Where(c => c.CapacidadMaxima == clasesQueryFilter.CapacidadMaxima);
            }
           
            var pagedClases = PagedList<object>.Create(clases, clasesQueryFilter.PageNumber, clasesQueryFilter.PageSize);
            if(pagedClases.Any()){
                return new ResponseData()
                {
                    Messages = new Message[] { new()
                    {
                        Type = "Information",
                        Description = "Registros de clase obtenidos"
                    } },
                    Pagination = pagedClases,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            else{
                return new ResponseData()
                {
                    Messages = new Message[] { new()
                    {
                        Type = "Warning",
                        Description = "No se encontraron registros de clase"
                    } },
                    Pagination = pagedClases,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
        }

        public async Task<IEnumerable<Clase>> GetAllClaseDapperAsync()
        {
            return await _unitOfWork.claseRepository.GetAllClaseDapperAsync();
        }
        public async Task<Clase> GetClaseByIdAsync(int id)
        {
            return await _unitOfWork.claseRepository.GetByIdAsync(id);
        }

        public async Task InsertClase(Clase clase)
        {
            await _unitOfWork.claseRepository.AddAsync(clase);
        }

        public async Task UpdateClaseAsync(Clase clase)
        {
            await _unitOfWork.claseRepository.UpdateAsync(clase);
        }

        public async Task DeleteClaseAsync(Clase clase)
        {
            await _unitOfWork.claseRepository.DeleteAsync(clase);
        }

        public async Task<IEnumerable<Clase>> GetClaseByInstructorAsync(int instructorId)
        {
            return await _unitOfWork.claseRepository.GetClaseByInstructorAsync(instructorId);
        }

        public async Task<IEnumerable<ClaseInstructorResponse>> GetClaseInstructor()
        {
            return await _unitOfWork.claseRepository.GetClaseInstructor();
        }
    
    }
}