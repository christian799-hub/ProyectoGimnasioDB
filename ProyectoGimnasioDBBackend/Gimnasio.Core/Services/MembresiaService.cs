
using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.QueryFilters;

namespace Gimnasio.Core.Services
{
    public class MembresiaService : IMembresiaService
    {
        public readonly IUnitOfWork _unitOfWork;
        public MembresiaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData> GetAllMembresiasAsync(
            MembresiaQueryFilter membresiaQueryFilter)
        {
            var membresias = await _unitOfWork.membresiaRepository.GetAllAsync();

            if(membresiaQueryFilter.Descripcion != null)
            {
                membresias = membresias.Where(m => m.Descripcion.Contains(membresiaQueryFilter.Descripcion));
            }
            if(membresiaQueryFilter.Precio != null)
            {
                membresias = membresias.Where(m => m.Precio == membresiaQueryFilter.Precio);
            }
            if(membresiaQueryFilter.DuracionDias != null)
            {
                membresias = membresias.Where(m => m.DuracionDias == membresiaQueryFilter.DuracionDias);
            }

            var pagedMembresia = PagedList<object>.Create(membresias, membresiaQueryFilter.PageNumber, membresiaQueryFilter.PageSize);
            if(pagedMembresia.Any()){
                return new ResponseData()
                {
                    Messages = new Message[] { new()
                    {
                        Type = "Information",
                        Description = "Registros de clase obtenidos"
                    } },
                    Pagination = pagedMembresia,
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
                    Pagination = pagedMembresia,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
        }
        public async Task<IEnumerable<Membresia>> GetAllClaseDapperAsync()
        {
            return await _unitOfWork.membresiaRepository.GetAllClaseDapperAsync();
        }
        public async Task<Membresia> GetMembresiaByIdAsync(int id)
        {
            return await _unitOfWork.membresiaRepository.GetByIdAsync(id);
        }
        public async Task InsertMembresia(Membresia membresia)
        {
            
            await _unitOfWork.membresiaRepository.AddAsync(membresia);
        }
        public async Task UpdateMembresiaAsync(Membresia membresia)
        {
            await _unitOfWork.membresiaRepository.UpdateAsync(membresia);
        }
        public async Task DeleteMembresiaAsync(Membresia membresia)
        {
            await _unitOfWork.membresiaRepository.DeleteAsync(membresia);
        }

        public async Task<IEnumerable<MembresiaOrdenPrecioResponse>> GetMembresiaOrdenPrecio()
        {
            return await _unitOfWork.membresiaRepository.GetMembresiaOrdenPrecio();
        }
    }
}