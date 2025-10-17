
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;

namespace Gimnasio.Core.Services
{
    public class UsuarioMembresiaService : IUsuarioMembresiaService
    {
        public readonly IUsuarioMembresiasRepository _usuarioMembresiaRepository;
        public readonly IUsuariosRepository _usuarioRepository;
        public readonly IMembresiasRepository _membresiaRepository;
        public UsuarioMembresiaService(IUsuarioMembresiasRepository usuarioMembresiaRepository,
         IUsuariosRepository usuarioRepository, IMembresiasRepository membresiaRepository)
        {
            _usuarioMembresiaRepository = usuarioMembresiaRepository;
            _usuarioRepository = usuarioRepository;
            _membresiaRepository = membresiaRepository;
        }
        public async Task<IEnumerable<UsuarioMembresia>> GetAllUsuarioMembresiasAsync()
        {
            return await _usuarioMembresiaRepository.GetAllUsuarioMembresiasAsync();
        }
        public async Task<UsuarioMembresia> GetUsuarioMembresiaByIdAsync(int id)
        {
            return await _usuarioMembresiaRepository.GetUsuarioMembresiaByIdAsync(id);
        }
        public async Task InsertUsuarioMembresia(UsuarioMembresia usuarioMembresia)
        {
            var usuario = await _usuarioRepository.GetUsuariosByIdAsync(usuarioMembresia.UsuarioId);
            var membresia = await _membresiaRepository.GetMembresiaByIdAsync(usuarioMembresia.MembresiaId);
            if (usuario == null)
            {
                throw new Exception("El usuario no existe.");
            }
            if (membresia == null)
            {
                throw new Exception("La membresía no existe.");
            }

            usuario.IsActive = 1; // Activar usuario cada vez que se le asigne una membresia
            await _usuarioMembresiaRepository.InsertarUsuarioMembresia(usuarioMembresia);
        }
        public async Task UpdateUsuarioMembresiaAsync(UsuarioMembresia usuarioMembresia)
        {
            await _usuarioMembresiaRepository.UpdateUsuarioMembresia(usuarioMembresia);
        }
        public async Task DeleteUsuarioMembresiaAsync(UsuarioMembresia usuarioMembresia)
        {
            await _usuarioMembresiaRepository.DeleteUsuarioMembresia(usuarioMembresia);
        }

        public async Task<bool> UsuarioTieneMembresiaActivaAsync(int usuarioId)
        {
            return await _usuarioMembresiaRepository.UsuarioTieneMembresiaActivaAsync(usuarioId);
        }

        public async Task<UsuarioMembresia> GetMembresiaActivaPorUsuarioIdAsync(int usuarioId)
        {
            return await _usuarioMembresiaRepository.GetMembresiaActivaByUsuarioAsync(usuarioId);
        }

    }
}