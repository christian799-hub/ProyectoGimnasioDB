using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;
using Gimnasio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class MembresiaRepository : IMembresiasRepository
{
    private readonly GimnasioContext _context;

    public MembresiaRepository(GimnasioContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Membresia>> GetAllMembresiasAsync()
    {
        var membresias = await _context.Membresias.ToListAsync();
        return membresias;
    }
    public async Task<Membresia> GetMembresiaByIdAsync(int id)
    {
        var membresia = await _context.Membresias.FirstOrDefaultAsync(x => x.Id == id);
        return membresia;
    }

    public async Task InsertarMembresia(Membresia membresia)
    {
        _context.Membresias.Add(membresia);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMembresia(Membresia membresia)
    {
        _context.Membresias.Update(membresia);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMembresia(Membresia membresia)
    {
        _context.Membresias.Remove(membresia);
        await _context.SaveChangesAsync();
    }
}
}
