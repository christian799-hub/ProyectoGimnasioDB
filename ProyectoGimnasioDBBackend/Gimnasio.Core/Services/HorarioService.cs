
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;

namespace Gimnasio.Core.Services
{
    public class HorarioService : IHorarioService
    {
        public readonly IHorariosRepository _horarioRepository;
        public readonly IClasesRepository _clasesRepository;
        public HorarioService(IHorariosRepository horarioRepository, IClasesRepository clasesRepository)
        {
            _horarioRepository = horarioRepository;
            _clasesRepository = clasesRepository;
        }
        public async Task<IEnumerable<Horario>> GetAllHorariosAsync()
        {
            return await _horarioRepository.GetAllHorariosAsync();
        }
        public async Task<Horario> GetHorarioByIdAsync(int id)
        {
            return await _horarioRepository.GetHorarioByIdAsync(id);
        }
        public async Task InsertHorario(Horario horario)
        {

            //Validar clase 

            var clase = await _clasesRepository.GetClaseByIdAsync(horario.ClaseId);
            if (clase == null)
            {
                throw new Exception("Clase no encontrada");
            }
            if (clase.IsActive == 0) //Esta activa?
            {
                throw new Exception("No se puede asignar un horario a una clase inactiva");
            }
            
            await _horarioRepository.InsertarHorario(horario);
        }
        public async Task UpdateHorarioAsync(Horario horario)
        {
            await _horarioRepository.UpdateHorario(horario);
        }
        public async Task DeleteHorarioAsync(Horario horario)
        {
            await _horarioRepository.DeleteHorario(horario);
        }
        public async Task<IEnumerable<Horario>> GetHorariosByClaseAsync(int usuarioId)
        {
            return await _horarioRepository.GetHorariosByClaseAsync(usuarioId);
        }
    }
}