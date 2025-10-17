
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;

namespace Gimnasio.Core.Services
{
    public class MembresiaService : IMembresiaService
    {
        public readonly IMembresiasRepository _membresiaRepository;
        public MembresiaService(IMembresiasRepository membresiaRepository)
        {
            _membresiaRepository = membresiaRepository;
        }
        public async Task<IEnumerable<Membresia>> GetAllMembresiasAsync()
        {
            return await _membresiaRepository.GetAllMembresiasAsync();
        }
        public async Task<Membresia> GetMembresiaByIdAsync(int id)
        {
            return await _membresiaRepository.GetMembresiaByIdAsync(id);
        }
        public async Task InsertMembresia(Membresia membresia)
        {
            
            await _membresiaRepository.InsertarMembresia(membresia);
        }
        public async Task UpdateMembresiaAsync(Membresia membresia)
        {
            await _membresiaRepository.UpdateMembresia(membresia);
        }
        public async Task DeleteMembresiaAsync(Membresia membresia)
        {
            await _membresiaRepository.DeleteMembresia(membresia);
        }
    }
}