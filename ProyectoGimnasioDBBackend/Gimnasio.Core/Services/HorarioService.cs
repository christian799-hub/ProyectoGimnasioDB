
using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Exceptions;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.QueryFilters;
using System; // Add this line to include the System namespace

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
        public async Task<ResponseData> GetAllHorariosAsync(
            HorarioQueryFilter horarioQueryFilter
        )
        {
            var horarios = await _horarioRepository.GetAllAsync();
            if (horarioQueryFilter.ClaseId != null)
            {
                horarios = horarios.Where(h => h.ClaseId == horarioQueryFilter.ClaseId);
            }
            if (horarioQueryFilter.DiaSemana != null)
            {
                horarios = horarios.Where(h => h.DiaSemana == horarioQueryFilter.DiaSemana.ToString());
            }
            if(horarioQueryFilter.Sala != null)
            {
                horarios = horarios.Where(h => h.Sala.Contains(horarioQueryFilter.Sala));
            }

            var pagedHorarios = PagedList<object>.Create(horarios, horarioQueryFilter.PageNumber, horarioQueryFilter.PageSize);
            if(pagedHorarios.Any()){
                return new ResponseData()
                {
                    Messages = new Message[] { new()
                    {
                        Type = "Information",
                        Description = "Registros de clase obtenidos"
                    } },
                    Pagination = pagedHorarios,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            else{
                return new ResponseData()
                {
                    Messages = new Message[] { new()
                    {
                        Type = "Warning",
                        Description = "No se encontraron registros de clase"
                    } },
                    Pagination = pagedHorarios,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
        }

        public async Task<IEnumerable<Horario>> GetAllClaseDapperAsync()
        {
            return await _horarioRepository.GetAllClaseDapperAsync();
        }
        public async Task<Horario> GetHorarioByIdAsync(int id)
        {
            return await _horarioRepository.GetByIdAsync(id);
        }
        public async Task InsertHorario(Horario horario)
        {

            //Validar clase 

            var clase = await _clasesRepository.GetByIdAsync(horario.ClaseId);
            if (clase == null)
            {
                throw new BussinesException("Clase no encontrada");
            }
            if (clase.IsActive == 0) //Esta activa?
            {
                throw new BussinesException("No se puede asignar un horario a una clase inactiva");
            }
            
            await _horarioRepository.AddAsync(horario);
        }
        public async Task UpdateHorarioAsync(Horario horario)
        {
            await _horarioRepository.UpdateAsync(horario);
        }
        public async Task DeleteHorarioAsync(Horario horario)
        {
            await _horarioRepository.DeleteAsync(horario);
        }
        public async Task<IEnumerable<Horario>> GetHorariosByClaseAsync(int usuarioId)
        {
            return await _horarioRepository.GetHorariosByClaseAsync(usuarioId);
        }

        public async Task<IEnumerable<HorarioDisponibilidadResponse>> GetHorariosDisponibilidadAsync()
        {
            return await _horarioRepository.GetHorariosDisponibilidadAsync();
        }
    }
}