
using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Exceptions;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.QueryFilters;

namespace Gimnasio.Core.Services
{
    public class UsuarioMembresiaService : IUsuarioMembresiaService
    {
        public readonly IUnitOfWork _unitOfWork;
        public UsuarioMembresiaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> GetAllUsuarioMembresiasAsync(
            UsuarioMembresiaQueryFilter usuarioMembresiaQueryFilter
        )
        {
            var usuarioMembresias = await _unitOfWork.usuarioMembresiasRepository.GetAllAsync();
            if (usuarioMembresiaQueryFilter.UsuarioId != null)
            {
                usuarioMembresias = usuarioMembresias.Where(um => um.UsuarioId == usuarioMembresiaQueryFilter.UsuarioId); 
            }
            if (usuarioMembresiaQueryFilter.MembresiaId != null)
            {
                usuarioMembresias = usuarioMembresias.Where(um => um.MembresiaId == usuarioMembresiaQueryFilter.MembresiaId);              
            }
            if(usuarioMembresiaQueryFilter.ClasesRestantes != null)
            {
                usuarioMembresias = usuarioMembresias.Where(um => um.ClasesRestantes == usuarioMembresiaQueryFilter.ClasesRestantes);
            }

            var pagedUsuarioMembresia = PagedList<object>.Create(usuarioMembresias, usuarioMembresiaQueryFilter.PageNumber, usuarioMembresiaQueryFilter.PageSize);
            if(pagedUsuarioMembresia.Any()){
                return new ResponseData()
                {
                    Messages = new Message[] { new()
                    {
                        Type = "Information",
                        Description = "Registros de clase obtenidos"
                    } },
                    Pagination = pagedUsuarioMembresia,
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
                    Pagination = pagedUsuarioMembresia,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
        }

        public async Task<IEnumerable<UsuarioMembresia>> GetAllUsuariosDapperAsync()
        {
            return await _unitOfWork.usuarioMembresiasRepository.GetAllUsuariosDapperAsync();
        }
        public async Task<UsuarioMembresia> GetUsuarioMembresiaByIdAsync(int id)
        {
            return await _unitOfWork.usuarioMembresiasRepository.GetByIdAsync(id);
        }
        public async Task InsertUsuarioMembresia(UsuarioMembresia usuarioMembresia)
        {
            // Validar usuario y membresia 
            var usuario = await _unitOfWork.usuarioRepository.GetByIdAsync(usuarioMembresia.UsuarioId);
            var membresia = await _unitOfWork.membresiaRepository.GetByIdAsync(usuarioMembresia.MembresiaId);
            if (usuario == null)
            {
                throw new BussinesException("El usuario no existe.");
            }
            if (membresia == null)
            {
                throw new BussinesException("La membresía no existe.");
            }

            usuario.IsActive = 1; // Activar usuario cada vez que se le asigne una membresia
            await _unitOfWork.usuarioRepository.UpdateAsync(usuario); //Actualiza al usuario
            await _unitOfWork.usuarioMembresiasRepository.AddAsync(usuarioMembresia);
        }
        public async Task UpdateUsuarioMembresiaAsync(UsuarioMembresia usuarioMembresia)
        {
            await _unitOfWork.usuarioMembresiasRepository.UpdateAsync(usuarioMembresia);
        }
        public async Task DeleteUsuarioMembresiaAsync(UsuarioMembresia usuarioMembresia)
        {
            await _unitOfWork.usuarioMembresiasRepository.DeleteAsync(usuarioMembresia);
        }

        public async Task<bool> UsuarioTieneMembresiaActivaAsync(int usuarioId)
        {
            return await _unitOfWork.usuarioMembresiasRepository.UsuarioTieneMembresiaActivaAsync(usuarioId);
        }

        public async Task<UsuarioMembresia> GetMembresiaActivaPorUsuarioIdAsync(int usuarioId)
        {
            return await _unitOfWork.usuarioMembresiasRepository.GetMembresiaActivaByUsuarioAsync(usuarioId);
        }

        public async Task<IEnumerable<UsuarioYMembresiaResponse>> GetUsuarioYMembresia()
        {
            return await _unitOfWork.usuarioMembresiasRepository.GetUsuarioYMembresia();
        }

    }
}