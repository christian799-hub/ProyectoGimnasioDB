
using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.QueryFilters;

namespace Gimnasio.Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        public readonly IUnitOfWork _unitOfWork;
        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData> GetAllUsuariosAsync(
            UsuarioQueryFilter usuarioQueryFilter
        )
        {
            var usuarios = await _unitOfWork.usuarioRepository.GetAllAsync();
            if (usuarioQueryFilter.Nombre != null)
            {
                usuarios = usuarios.Where(u => u.Nombre == usuarioQueryFilter.Nombre);
            }
            if (usuarioQueryFilter.Telefono != null)
            {
                usuarios = usuarios.Where(u => u.Telefono == usuarioQueryFilter.Telefono);
            }
            if (usuarioQueryFilter.Edad != null)
            {
                usuarios = usuarios.Where(u => u.Edad == usuarioQueryFilter.Edad);
            }
            
            var pagedUsuario = PagedList<object>.Create(usuarios, usuarioQueryFilter.PageNumber, usuarioQueryFilter.PageSize);
            if(pagedUsuario.Any()){
                return new ResponseData()
                {
                    Messages = new Message[] { new()
                    {
                        Type = "Information",
                        Description = "Registros de clase obtenidos"
                    } },
                    Pagination = pagedUsuario,
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
                    Pagination = pagedUsuario,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosDapperAsync()
        {
            return await _unitOfWork.usuarioRepository.GetAllUsuariosDapperAsync();
        }
        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _unitOfWork.usuarioRepository.GetByIdAsync(id);
        }
        public async Task InsertUsuario(Usuario usuario)
        {
            await _unitOfWork.usuarioRepository.AddAsync(usuario);
        }
        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            await _unitOfWork.usuarioRepository.UpdateAsync(usuario);
        }
        public async Task DeleteUsuarioAsync(Usuario usuario)
        {
            await _unitOfWork.usuarioRepository.DeleteAsync(usuario);
        }
        public async Task<IEnumerable<UsuarioAsistenciaResponse>> GetAsistenciasUsuarios()
        {
            return await _unitOfWork.usuarioRepository.GetAsistenciasUsuarios();
        }
        
    }
}