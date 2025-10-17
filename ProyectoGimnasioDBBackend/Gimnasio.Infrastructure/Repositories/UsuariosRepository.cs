using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;
using Gimnasio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class UsuariosRepository : IUsuariosRepository
{
    private readonly GimnasioContext _context;

    public UsuariosRepository(GimnasioContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return usuarios;
    }
    public async Task<Usuario> GetUsuariosByIdAsync(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync( x=> x.Id == id);
        return usuario;
    }

    public async Task<IEnumerable<Usuario>> GetAllUsuariosActivosAsync(){
        var usuarios = _context.Usuarios.Where(u => u.IsActive == 1);
        return usuarios;
    }

    public async Task InsertarUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUsuarios(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUsuarios(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<UsuarioMembresia> GetMembresiaActivaByUsuarioAsyncConFecha(int usuarioId, DateOnly fecha)
    {
    return await _context.UsuarioMembresias
        .Include(um => um.Membresia)
        .Include(um => um.Usuario)
        .FirstOrDefaultAsync(um => um.UsuarioId == usuarioId && 
                                  um.Estado == "Activa" && 
                                  um.FechaInicio <= fecha &&
                                  um.FechaFin >= fecha); 
    }
}


}
