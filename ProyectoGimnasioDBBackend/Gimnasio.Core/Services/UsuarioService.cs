
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;

namespace Gimnasio.Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        public readonly IUsuariosRepository _usuarioRepository;
        public UsuarioService(IUsuariosRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _usuarioRepository.GetAllUsuariosAsync();
        }
        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _usuarioRepository.GetUsuariosByIdAsync(id);
        }
        public async Task InsertUsuario(Usuario usuario)
        {
            await _usuarioRepository.InsertarUsuario(usuario);
        }
        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            await _usuarioRepository.UpdateUsuarios(usuario);
        }
        public async Task DeleteUsuarioAsync(Usuario usuario)
        {
            await _usuarioRepository.DeleteUsuarios(usuario);
        }
    }
}