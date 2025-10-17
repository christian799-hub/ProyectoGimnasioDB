using Microsoft.AspNetCore.Mvc;
using Gimnasio.Infrastructure.Data;
using Gimnasio.Core.Interfaces;
using Gimnasio.Infrastructure.Repositories;
using Gimnasio.Core.Entities;
using Gimnasio.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Gimnasio.Infrastructure.Validators;
using Gimnasio.Core.CustomEntities;
using Gimnasio.Api.Responses;
using System.Net;

namespace Gimnasio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciumController : ControllerBase
    {
        private readonly IAsistenciaService _asistenciaService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public AsistenciumController(IAsistenciaService asistenciaService,IMapper mapper, IValidationService validationService)
        {
            _asistenciaService = asistenciaService;
            _mapper = mapper;
            _validationService = validationService;
        }

        #region  CRUD SIN DTO
        
        [HttpGet]
        public async Task<IActionResult> GetAsistencia()
        {
            var asistencia = await _asistenciaService.GetAllAsistenciasAsync();
            return Ok(asistencia);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsistenciaByIdUsuario(int id)
        {
            var asistencia = await _asistenciaService.GetAsistenciaByIdAsync(id);
            return Ok(asistencia);
        }

        [HttpPost("RegistrarAsistencia/{id}/{fecha}")] //registra asistencias verificando que no haya un registro previo para el mismo usuario, horario y fecha
        public async Task<IActionResult> RegistrarAsistencia(int id, DateOnly fecha, [FromBody] Asistencium asistencia)
        {
            if (id != asistencia.UsuarioId)
            {
                return BadRequest("El ID del usuario no coincide con el ID en la asistencia.");
            }

            if (fecha != asistencia.FechaAsistencia)
            {
                return BadRequest("La fecha no coincide con la fecha en la asistencia.");
            }

            await _asistenciaService.InsertAsistencia(asistencia);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsistencia(Asistencium asistencia)
        {
            await _asistenciaService.UpdateAsistenciaAsync(asistencia);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsistencia(int id)
        {
            var result = await _asistenciaService.GetAsistenciaByIdAsync(id);

            if(result == null)
            {
                return NotFound();
            }

            await _asistenciaService.DeleteAsistenciaAsync(result);
            return NoContent();
        }
        /*

        [HttpPost("RegistrarAsistencia/")]
        public async Task<IActionResult> RegistrarAsistencia(int id, Asistencium asistencia)
        {
            if (id != asistencia.UsuarioId)
            {
                return BadRequest("El ID del usuario no coincide con el ID en la asistencia.");
            }

            await _asistenciaRepository.RegistrarAsistenciaAsync(asistencia);
            return Ok();
        }
        */

        #endregion

        #region CRUD CON DTO

        [HttpGet("dto")]
        public async Task<IActionResult> GetAsistenciaDto()
        {
            var asistencias = await _asistenciaService.GetAllAsistenciasAsync();
            var asistenciaDtos = asistencias.Select(a => new
            {
                a.Id,
                a.UsuarioId,
                a.HorarioId,
                a.FechaAsistencia,
                a.Estado,
                Usuario = a.Usuario != null ? new  // Valida null
                {
                    a.Usuario.Id,
                    a.Usuario.Nombre,
                    a.Usuario.Telefono
                } : null,  // Devuelve null si no existe
                Horario = a.Horario != null ? new  
                {
                    a.Horario.Id,
                    a.Horario.DiaSemana,
                    a.Horario.HoraInicio,
                    a.Horario.HoraFin,
                    Clase = a.Horario.Clase != null ? new  
                    {
                        a.Horario.Clase.Id,
                        a.Horario.Clase.Instructor,
                        a.Horario.Clase.Descripcion
                    } : null  
                } : null  
            });

            return Ok(asistenciaDtos);
        }

    [HttpGet("dto/{id}")]
    public async Task<IActionResult> GetAsistenciaByIdUsuarioDto(int id)
    {
        var asistencia = await _asistenciaService.GetAsistenciaByIdAsync(id);
        if (asistencia == null) return NotFound();

        var asistenciaDto = new
        {
            asistencia.Id,
            asistencia.UsuarioId,
            asistencia.HorarioId,
            asistencia.FechaAsistencia,
            asistencia.Estado,
            Usuario = asistencia.Usuario == null ? null : new
            {
                asistencia.Usuario.Id,
                asistencia.Usuario.Nombre,
                asistencia.Usuario.Telefono
            },
            Horario = asistencia.Horario == null ? null : new
            {
                asistencia.Horario.Id,
                asistencia.Horario.DiaSemana,
                asistencia.Horario.HoraInicio,
                asistencia.Horario.HoraFin,
                Clase = asistencia.Horario.Clase == null ? null : new
                {
                    asistencia.Horario.Clase.Id,
                    Instructor = asistencia.Horario.Clase.Instructor?.Nombre,
                    asistencia.Horario.Clase.Descripcion
                }
            }
        };

        return Ok(asistenciaDto);
    }

        [HttpPost("dto/RegistrarAsistencia")] 
        public async Task<IActionResult> RegistrarAsistenciaDto(AsistenciaDto registrarAsistenciaDto)
        {
            var asistencia = new Asistencium
            {
                UsuarioId = registrarAsistenciaDto.UsuarioId,
                HorarioId = registrarAsistenciaDto.HorarioId,
                FechaAsistencia = registrarAsistenciaDto.FechaAsistencia
            };

            await _asistenciaService.InsertAsistencia(asistencia);

            return Ok(asistencia);
        }

        [HttpPut("dto")]
        public async Task<IActionResult> UpdateAsistenciaDto(AsistenciaDto asistenciaDto)
        {
            var asistencia = await _asistenciaService.GetAsistenciaByIdAsync(asistenciaDto.Id ?? 0);
            if (asistencia == null)
            {
                return NotFound();
            }

            asistencia.UsuarioId = asistenciaDto.UsuarioId;
            asistencia.HorarioId = asistenciaDto.HorarioId;
            asistencia.FechaAsistencia = asistenciaDto.FechaAsistencia;
            asistencia.Estado = asistenciaDto.Estado;

            await _asistenciaService.UpdateAsistenciaAsync(asistencia);

            return Ok(asistencia);
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteAsistenciaDto(int id)
        {
            var asistencia = await _asistenciaService.GetAsistenciaByIdAsync(id);
            if (asistencia == null)
            {
                return NotFound();
            }

            await _asistenciaService.DeleteAsistenciaAsync(asistencia);
            return NoContent();
        }

        #endregion

        #region CRUD CON MAPPER Y VALIDACIONES

        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetAsistenciaDtoMapper()
        {
            var asistencias = await _asistenciaService.GetAllAsistenciasAsync();
            var asistenciaDtos = _mapper.Map<IEnumerable<AsistenciaDto>>(asistencias);

            var response = new ApiResponse<IEnumerable<AsistenciaDto>>(asistenciaDtos);

            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")] 
        public async Task<IActionResult> GetAsistenciaByIdUsuarioDtoMapper(int id)
        {
            var validationRequest = new GetByIdRequest { Id = id };

            var validationResult = await _validationService.ValidateAsync(validationRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(new 
                { 
                    Message = "Error de validación en el ID",
                    Errors = validationResult.Errors 
                });
            }

            var asistencia = await _asistenciaService.GetAsistenciaByIdAsync(id);

            if (asistencia == null) 
                return NotFound();

            var asistenciaDto = _mapper.Map<AsistenciaDto>(asistencia);

            var response = new ApiResponse<AsistenciaDto>(asistenciaDto);

            return Ok(response);
        }

        [HttpPost("dto/mapper/RegistrarAsistencia")] 
        public async Task<IActionResult> RegistrarAsistenciaDtoMapper(AsistenciaDto registrarAsistenciaDto)
        {
            try{

                var validationResult = await _validationService.ValidateAsync(registrarAsistenciaDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var asistencia = _mapper.Map<Asistencium>(registrarAsistenciaDto);
                await _asistenciaService.InsertAsistencia(asistencia);

                var response = new ApiResponse<Asistencium>(asistencia);

                return Ok(response);
            }
            catch(Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,err.Message);
            }
            
        }

        [HttpPut("dto/mapper")]
        public async Task<IActionResult> UpdateAsistenciaDtoMapper(AsistenciaDto asistenciaDto)
        {
            var asistencia = await _asistenciaService.GetAsistenciaByIdAsync(asistenciaDto.Id ?? 0);
            if (asistencia == null)
            {
                return NotFound();
            }
            _mapper.Map(asistenciaDto, asistencia);

            await _asistenciaService.UpdateAsistenciaAsync(asistencia);

            var response = new ApiResponse<Asistencium>(asistencia);

            return Ok(response);
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteAsistenciaDtoMapper(int id)
        {
            var asistencia = await _asistenciaService.GetAsistenciaByIdAsync(id);
            if (asistencia == null)
            {
                return NotFound();
            }

            await _asistenciaService.DeleteAsistenciaAsync(asistencia);

            return NoContent();
        }

        #endregion

        #region Métodos adicionales

         [HttpGet("GetAsistenciaByClase/{claseId}/{horario}/{fecha}")]
        public async Task<IActionResult> GetAsistenciaByClase(int claseId, int horario, DateOnly fecha)
        {
            var asistencias = await _asistenciaService.GetAsistenciaByClaseAsync(claseId, fecha);

            var response = new ApiResponse<IEnumerable<Asistencium>>(asistencias);

            return Ok(response);
        }

        [HttpGet("UsuarioYaRegistroAsistencia/{usuarioId}/{horarioId}/{fecha}")]
        public async Task<IActionResult> UsuarioYaRegistroAsistencia(int usuarioId, int horarioId, DateOnly fecha)
        {
            var asistencia = await _asistenciaService.UsuarioYaRegistroAsistenciaAsync(usuarioId, horarioId, fecha);

            var response = new ApiResponse<bool>(asistencia);

            return Ok(response);
        }

        [HttpGet("GetAsistenciaByIdUsuario/{usuarioId}")]
        public async Task<IActionResult> GetAsistenciaUsuario(int usuarioId)
        {
            var asistencia = await _asistenciaService.GetAsistenciaByIdUsuarioAsync(usuarioId);

            var response = new ApiResponse<IEnumerable<Asistencium>>(asistencia);

            return Ok(response);
        }

        #endregion
    }
}