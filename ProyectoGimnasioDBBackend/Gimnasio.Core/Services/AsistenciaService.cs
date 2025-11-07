
using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Exceptions;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.QueryFilters;

namespace Gimnasio.Core.Services
{
   public class AsistenciaService : IAsistenciaService
   {
      
      public readonly IUnitOfWork _unitOfWork;

      public AsistenciaService(IUnitOfWork unitOfWork)
      {
         _unitOfWork = unitOfWork;
      }

      public async Task<ResponseData> GetAllAsistenciasAsync(
        AsistenciaQueryFilter asistenciaQueryFilter
      )
      {
        var asistencias = await _unitOfWork.asistenciaRepository.GetAllAsync();

        if (asistenciaQueryFilter.UsuarioId != null)
        {
            asistencias = asistencias.Where(a => a.UsuarioId == asistenciaQueryFilter.UsuarioId);
        }
        if (asistenciaQueryFilter.HorarioId != null)
        {
            asistencias = asistencias.Where(a => a.HorarioId == asistenciaQueryFilter.HorarioId);
        }
        if (asistenciaQueryFilter.FechaAsistencia != default(DateOnly))
        {
            asistencias = asistencias.Where(a => a.FechaAsistencia == asistenciaQueryFilter.FechaAsistencia);
        }

        var pagedAsistencias = PagedList<object>.Create(asistencias, asistenciaQueryFilter.PageNumber, asistenciaQueryFilter.PageSize);
            if(pagedAsistencias.Any()){
                return new ResponseData()
                {
                    Messages = new Message[] { new()
                    {
                        Type = "Information",
                        Description = "Registros de clase obtenidos"
                    } },
                    Pagination = pagedAsistencias,
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
                    Pagination = pagedAsistencias,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
      }
      public async Task<IEnumerable<Asistencium>> GetAllAsistenciasDapperAsync()
      {
         return await _unitOfWork.asistenciaRepository.GetAllAsistenciasDapperAsync();
      }
      public async Task<Asistencium> GetAsistenciaByIdAsync(int id)
      {
         return await _unitOfWork.asistenciaRepository.GetByIdAsync(id);
      }

    public async Task InsertAsistencia(Asistencium asistencia)
    {
    //Validar Usuario y Horario
    var usuario = await _unitOfWork.usuarioRepository.GetByIdAsync(asistencia.UsuarioId);
    var horario = await _unitOfWork.horariosRepository.GetByIdAsync(asistencia.HorarioId);
    
    if (usuario == null)
        throw new BussinesException("Usuario no encontrado");
    
    if (usuario.IsActive == 0)
        throw new BussinesException("Usuario inactivo");
    
    if (horario == null)
        throw new BussinesException("Horario no encontrado");

    var clase = await _unitOfWork.claseRepository.GetByIdAsync(horario.ClaseId);

    if (clase == null)
        throw new BussinesException("El horario no tiene una clase asociada");

    // Validar si el usuario ya registró asistencia para esa clase en la fecha dada
    var yaRegistro = await UsuarioYaRegistroAsistenciaAsync(asistencia.UsuarioId, asistencia.HorarioId, asistencia.FechaAsistencia);

    if (yaRegistro)
        throw new BussinesException("El usuario ya ha registrado asistencia para esta clase en la fecha indicada.");

    // Validar capacidad de la clase
    var capacidadMaxima = await _unitOfWork.claseRepository.GetCapacidadMaximaAsync(horario.ClaseId);
    // Obtener la cantidad de asistencias ya registradas para ese horario y fecha
    var asistenciasRegistradas = await _unitOfWork.asistenciaRepository.GetCantidadAsistenciasByHorarioAndFechaAsync(asistencia.HorarioId, asistencia.FechaAsistencia);
    
     if (asistenciasRegistradas >= capacidadMaxima)
         throw new BussinesException("La clase ha alcanzado su capacidad máxima para la fecha indicada.");

    // Validar membresía en base a la fecha de asistencia

     var membresiaActiva = await _unitOfWork.usuarioMembresiasRepository
         .GetMembresiaActivaByUsuarioAndFechaAsync(asistencia.UsuarioId, asistencia.FechaAsistencia);
    
      if (membresiaActiva == null)
          throw new BussinesException("El usuario no tiene una membresia activa");
    
    // Validar clases restantes en la membresía

    var clasesIncluidas = membresiaActiva.Membresia?.ClasesIncluidas ?? 0;
    
    if (clasesIncluidas <= 0)
        throw new BussinesException("La membresía no incluye clases");

    if (membresiaActiva.ClasesRestantes <= 0)
         throw new BussinesException("No quedan clases disponibles en la membresía");


    membresiaActiva.ClasesRestantes -= 1;

    asistencia.Estado = "Presente";

    //actualizar valores cambiados 

    await _unitOfWork.usuarioMembresiasRepository.UpdateAsync(membresiaActiva);
    await _unitOfWork.usuarioRepository.UpdateAsync(usuario);

    await _unitOfWork.asistenciaRepository.AddAsync(asistencia);

}
      public async Task UpdateAsistenciaAsync(Asistencium asistencia)
      {
         await _unitOfWork.asistenciaRepository.UpdateAsync(asistencia);
      }

      public async Task DeleteAsistenciaAsync(Asistencium asistencia)
      {
        await _unitOfWork.asistenciaRepository.DeleteAsync(asistencia);
      }
      public async Task<IEnumerable<Asistencium>> GetAsistenciaByClaseAsync(int claseId,DateOnly fecha)
      {
        return await _unitOfWork.asistenciaRepository.GetAsistenciaByClaseAsync(claseId,fecha);
      }

      public async Task<IEnumerable<Asistencium>> GetAsistenciaByIdUsuarioAsync(int usuarioId)
      {
          return await _unitOfWork.asistenciaRepository.GetAsistenciaByIdUsuarioAsync(usuarioId);
      }

      public async Task<bool> UsuarioYaRegistroAsistenciaAsync(int usuarioId, int horarioId, DateOnly fecha)
      {
          return await _unitOfWork.asistenciaRepository.UsuarioYaRegistroAsistenciaAsync(usuarioId,horarioId,fecha);
      }

      public async Task<IEnumerable<AsistenciaCompletaResponse>> GetAsistenciaCompleta()
        {
            return await _unitOfWork.asistenciaRepository.GetAsistenciaCompleta();
        }

   }
}