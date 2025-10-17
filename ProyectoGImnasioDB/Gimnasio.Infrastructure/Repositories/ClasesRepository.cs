using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;
using Gimnasio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class ClasesRepository : IClasesRepository
{
    private readonly GimnasioContext _context;
    public ClasesRepository(GimnasioContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Clase>> GetAllClasesAsync()
    {
        var clases = await _context.Clases
        .Include(c => c.Instructor)    
        .Include(c => c.Horarios)
        .ToListAsync();
        return clases;
    }

    public async Task<Clase> GetClaseByIdAsync(int id)
    {
        var clase = await _context.Clases
        .Include(c => c.Instructor)    
        .Include(c => c.Horarios)
        .FirstOrDefaultAsync(x => x.Id == id);
        return clase;
    }

    public async Task<IEnumerable<Clase>> GetClaseByInstructorAsync(int instructorId)
    {
        var clases = await _context.Clases
        .Where(x => x.InstructorId == instructorId)
        .Include(c => c.Instructor)
        .Include(c => c.Horarios)
        .ToListAsync();
        return clases;
    }
    public async Task InsertarClase(Clase clase)
    {
        _context.Clases.Add(clase);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateClase(Clase clase)
    {
        _context.Clases.Update(clase);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteClase(Clase clase)
    {
        _context.Clases.Remove(clase);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetCapacidadMaximaAsync(int claseId)
    {
        var clase = await _context.Clases
        .FirstOrDefaultAsync(c => c.Id == claseId);
        if (clase == null)
        {
            throw new Exception("Clase no encontrada");
        }
        return clase.CapacidadMaxima;
    }
    
}
}
