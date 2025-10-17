
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;
using Gimnasio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class UsuarioMembresiasRepository : IUsuarioMembresiasRepository
{
    private readonly GimnasioContext _context;

    public UsuarioMembresiasRepository(GimnasioContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UsuarioMembresia>> GetAllUsuarioMembresiasAsync()
    {
        var usuarioMembresias = await _context.UsuarioMembresias
        .Include(um => um.Usuario)
        .Include(um => um.Membresia)
        .ToListAsync();
        return usuarioMembresias;
    }

    public async Task<UsuarioMembresia> GetUsuarioMembresiaByIdAsync(int id)
    {
        var usuarioMembresia = await _context.UsuarioMembresias
        .Include(um => um.Usuario)
        .Include(um => um.Membresia)
        .FirstOrDefaultAsync( x=> x.Id == id);
        return usuarioMembresia;
    }

    public async Task InsertarUsuarioMembresia(UsuarioMembresia usuarioMembresia)
    {
        _context.UsuarioMembresias.Add(usuarioMembresia);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUsuarioMembresia(UsuarioMembresia usuarioMembresia)
    {
        _context.UsuarioMembresias.Update(usuarioMembresia);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUsuarioMembresia(UsuarioMembresia usuarioMembresia)
    {
        _context.UsuarioMembresias.Remove(usuarioMembresia);
        await _context.SaveChangesAsync();
    }

        public async Task<bool> UsuarioTieneMembresiaActivaAsync(int usuarioId) // Verifica si un usuario tiene una membresia activa
    {
        return await _context.UsuarioMembresias
            .AnyAsync(um => um.UsuarioId == usuarioId && 
                           um.Estado == "Activa" && 
                           um.FechaFin >= DateOnly.FromDateTime(DateTime.Today));
    }

    public async Task<UsuarioMembresia> GetMembresiaActivaByUsuarioAsync(int usuarioId) // Obtiene la membresía activa de un usuario 
    {
        return await _context.UsuarioMembresias
            .Include(um => um.Membresia)
            .Include(um => um.Usuario)
            .FirstOrDefaultAsync(um => um.UsuarioId == usuarioId && 
                                      um.Estado == "Activa" && 
                                      um.FechaFin >= DateOnly.FromDateTime(DateTime.Today));
    }

    public async Task<UsuarioMembresia> GetMembresiaActivaByUsuarioAndFechaAsync(int usuarioId, DateOnly fecha)
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
