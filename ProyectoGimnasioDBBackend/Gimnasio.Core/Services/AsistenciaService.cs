
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;

namespace Gimnasio.Core.Services
{
   public class AsistenciaService : IAsistenciaService
   {
      private readonly IAsistenciaRepository _asistenciaRepository;
      public readonly IUsuariosRepository _usuarioRepository;
      public readonly IHorariosRepository _horarioRepository;
      public readonly IClasesRepository _claseRepository;
      public readonly IUsuarioMembresiasRepository _usuarioMembresiasRepository;

      public AsistenciaService(IAsistenciaRepository asistenciaRepository, IUsuariosRepository usuarioRepository,
       IHorariosRepository horarioRepository, IClasesRepository claseRepository, IUsuarioMembresiasRepository usuarioMembresiasRepository)
      {
         _asistenciaRepository = asistenciaRepository;
         _usuarioRepository = usuarioRepository;
         _horarioRepository = horarioRepository;
         _claseRepository = claseRepository;
         _usuarioMembresiasRepository = usuarioMembresiasRepository;
      }

      public async Task<IEnumerable<Asistencium>> GetAllAsistenciasAsync()
      {
         return await _asistenciaRepository.GetAllAsistenciasAsync();
      }
      public async Task<Asistencium> GetAsistenciaByIdAsync(int id)
      {
         return await _asistenciaRepository.GetAsistenciaByIdAsync(id);
      }

public async Task InsertAsistencia(Asistencium asistencia)
{
    //Validar Usuario y Horario
    var usuario = await _usuarioRepository.GetUsuariosByIdAsync(asistencia.UsuarioId);
    var horario = await _horarioRepository.GetHorarioByIdAsync(asistencia.HorarioId);
    
    if (usuario == null)
        throw new Exception("Usuario no encontrado");
    
    if (usuario.IsActive == 0)
        throw new Exception("Usuario inactivo");
    
    if (horario == null)
        throw new Exception("Horario no encontrado");
    
    if (horario.Clase == null)
        throw new Exception("El horario no tiene una clase asociada");

    // Validar si el usuario ya registró asistencia para esa clase en la fecha dada
    var yaRegistro = await UsuarioYaRegistroAsistenciaAsync(asistencia.UsuarioId, asistencia.HorarioId, asistencia.FechaAsistencia);

    if (yaRegistro)
        throw new Exception("El usuario ya ha registrado asistencia para esta clase en la fecha indicada.");

    // Validar capacidad de la clase
    var capacidadMaxima = await _claseRepository.GetCapacidadMaximaAsync(horario.ClaseId);
    // Obtener la cantidad de asistencias ya registradas para ese horario y fecha
    var asistenciasRegistradas = await _asistenciaRepository.GetCantidadAsistenciasByHorarioAndFechaAsync(asistencia.HorarioId, asistencia.FechaAsistencia);
    
     if (asistenciasRegistradas >= capacidadMaxima)
         throw new Exception("La clase ha alcanzado su capacidad máxima para la fecha indicada.");

    // Validar membresía en base a la fecha de asistencia

     var membresiaActiva = await _usuarioMembresiasRepository
         .GetMembresiaActivaByUsuarioAndFechaAsync(asistencia.UsuarioId, asistencia.FechaAsistencia);
    
      if (membresiaActiva == null)
          throw new Exception("El usuario no tiene una membresia activa");
    
    // Validar clases restantes en la membresía

    var clasesIncluidas = membresiaActiva.Membresia?.ClasesIncluidas ?? 0;
    
    if (clasesIncluidas <= 0)
        throw new Exception("La membresía no incluye clases");

    if (membresiaActiva.ClasesRestantes <= 0)
         throw new Exception("No quedan clases disponibles en la membresía");


    membresiaActiva.ClasesRestantes -= 1;

    asistencia.Estado = "Presente";

    //actualizar valores cambiados 

    await _usuarioMembresiasRepository.UpdateUsuarioMembresia(membresiaActiva);
    await _usuarioRepository.UpdateUsuarios(usuario);

    await _asistenciaRepository.InsertarAsistencia(asistencia);
}
      public async Task UpdateAsistenciaAsync(Asistencium asistencia)
      {
         await _asistenciaRepository.UpdateAsistencia(asistencia);
      }

      public async Task DeleteAsistenciaAsync(Asistencium asistencia)
      {
        await _asistenciaRepository.DeleteAsistencia(asistencia);
      }
      public async Task<IEnumerable<Asistencium>> GetAsistenciaByClaseAsync(int claseId,DateOnly fecha)
      {
        return await _asistenciaRepository.GetAsistenciaByClaseAsync(claseId,fecha);
      }

      public async Task<IEnumerable<Asistencium>> GetAsistenciaByIdUsuarioAsync(int usuarioId)
      {
          return await _asistenciaRepository.GetAsistenciaByIdUsuarioAsync(usuarioId);
      }

      public async Task<bool> UsuarioYaRegistroAsistenciaAsync(int usuarioId, int horarioId, DateOnly fecha)
      {
          return await _asistenciaRepository.UsuarioYaRegistroAsistenciaAsync(usuarioId,horarioId,fecha);
      }

   }
}