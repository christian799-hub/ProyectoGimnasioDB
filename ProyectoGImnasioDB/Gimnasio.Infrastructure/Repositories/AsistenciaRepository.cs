using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;
using Gimnasio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class AsistenciaRepository : IAsistenciaRepository
{
    private readonly GimnasioContext _context;

    public AsistenciaRepository(GimnasioContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Asistencium>> GetAllAsistenciasAsync()
    {
        var asistencias = await _context.Asistencia
        .Include(a => a.Usuario)      
        .Include(a => a.Horario)      
        .ThenInclude(h => h.Clase)
        .ThenInclude(c => c.Instructor)  
        .ToListAsync();
        return asistencias;
    }

   public async Task<Asistencium?> GetAsistenciaByIdAsync(int id)
    {
        var asistencia = await _context.Asistencia
            .Include(a => a.Usuario)      
            .Include(a => a.Horario)      
            .ThenInclude(h => h.Clase)
            .ThenInclude(c => c.Instructor)    
            .FirstOrDefaultAsync(a => a.Id == id);
        return asistencia;
    }

    public async Task InsertarAsistencia(Asistencium asistencia)
    {
        _context.Asistencia.Add(asistencia);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsistencia(Asistencium asistencia)
    {
        _context.Asistencia.Update(asistencia);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsistencia(Asistencium asistencia)
    {
        _context.Asistencia.Remove(asistencia);
        await _context.SaveChangesAsync();
    }

    public async Task RegistrarAsistenciaAsync(Asistencium asistencia)
    {
    var existeRegistro = await UsuarioYaRegistroAsistenciaAsync(
        asistencia.UsuarioId, 
        asistencia.HorarioId, 
        asistencia.FechaAsistencia  
    );

    if (!existeRegistro)
    {
        asistencia.Estado = "Presente";
        await InsertarAsistencia(asistencia);
    }
    else
    {
        throw new InvalidOperationException("El usuario ya registró asistencia para esta clase hoy.");
    }
    }

    public async Task<IEnumerable<Asistencium>> GetAsistenciaByClaseAsync(int claseId, DateOnly fecha)
    {
        return await _context.Asistencia
            .Include(a => a.Usuario)
            .Include(a => a.Horario)
            .Where(a => a.Horario.ClaseId == claseId &&
                       a.FechaAsistencia.Year == fecha.Year &&
                       a.FechaAsistencia.Month == fecha.Month &&
                       a.FechaAsistencia.Day == fecha.Day)
            .ToListAsync();
    }

    public async Task<bool> UsuarioYaRegistroAsistenciaAsync(int usuarioId, int horarioId, DateOnly fecha)
    {
    return await _context.Asistencia
        .AnyAsync(a => a.UsuarioId == usuarioId && 
                      a.HorarioId == horarioId && 
                      a.FechaAsistencia.Year == fecha.Year &&
                      a.FechaAsistencia.Month == fecha.Month &&
                      a.FechaAsistencia.Day == fecha.Day);

    }
        public async Task<IEnumerable<Asistencium>> GetAsistenciaByIdUsuarioAsync(int usuarioId)
    {
        var asistenciaUsuario = await _context.Asistencia
        .Where(x => x.UsuarioId == usuarioId).Include(a => a.Horario).ThenInclude(h => h.Clase)
        .OrderByDescending(a => a.FechaAsistencia)
        .ToListAsync();
        return asistenciaUsuario;
    }

    public async Task<int> GetCantidadAsistenciasByHorarioAndFechaAsync(int horarioId, DateOnly fecha)
    {
        return await _context.Asistencia
            .CountAsync(a => a.HorarioId == horarioId && 
                             a.FechaAsistencia.Year == fecha.Year &&
                             a.FechaAsistencia.Month == fecha.Month &&
                             a.FechaAsistencia.Day == fecha.Day);
    
    }
}
}
