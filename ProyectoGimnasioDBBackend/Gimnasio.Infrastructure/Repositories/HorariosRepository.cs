using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;
using Gimnasio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class HorariosRepository : IHorariosRepository
{
    private readonly GimnasioContext _context;

        public HorariosRepository(GimnasioContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Horario>> GetAllHorariosAsync()
    {
        var horarios = await _context.Horarios
            .Include(h => h.Clase)
            .ThenInclude(c => c.Instructor)  
            .ToListAsync();
        return horarios;
    }

    public async Task<Horario> GetHorarioByIdAsync(int id){
        var horario = await _context.Horarios
            .Include(h => h.Clase)
            .ThenInclude(c => c.Instructor)
            .FirstOrDefaultAsync(h => h.Id == id); 
        return horario;
    }
    public async Task InsertarHorario(Horario horario){
        _context.Horarios.Add(horario);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateHorario(Horario horario){
        _context.Horarios.Update(horario);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteHorario(Horario horario){
        _context.Horarios.Remove(horario);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Horario>> GetHorariosByClaseAsync(int claseId){
        var horarios = await _context.Horarios
            .Where(h => h.ClaseId == claseId)
            .Include(h => h.Clase)
            .ToListAsync();
        return horarios;
    }

}
}
