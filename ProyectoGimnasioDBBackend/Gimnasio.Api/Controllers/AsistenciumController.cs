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
using Gimnasio.Core.QueryFilters;

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

        #region CRUD CON MAPPER Y VALIDACIONES
        /// <summary>
        /// Recupera una lista paginada de asistencias y las mapea a DTOs utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de asistencias con soporte para paginación y mapeo automático a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <param name="asistenciaQueryFilter">Los filtros se aplicacn al recuperar las asistencias como la paginacion y busqueda, si no se envia los paramtetros se retornan todos los registros</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<AsistenciaDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetAsistenciaDtoMapper(
            [FromQuery]AsistenciaQueryFilter asistenciaQueryFilter )
        {
            var asistencias = await _asistenciaService.GetAllAsistenciasAsync(asistenciaQueryFilter);
            var asistenciaDtos = _mapper.Map<IEnumerable<AsistenciaDto>>(asistencias.Pagination);

            var pagination = new Pagination
            {
                TotalCount = asistencias.Pagination.TotalCount,
                PageSize = asistencias.Pagination.PageSize,
                CurrentPage = asistencias.Pagination.CurrentPage,
                TotalPages = asistencias.Pagination.TotalPages,
                HasNextPage = asistencias.Pagination.HasNextPage,
                HasPreviousPage = asistencias.Pagination.HasPreviousPage
            };


            var response = new ApiResponse<IEnumerable<AsistenciaDto>>(asistenciaDtos)
            {
                Pagination = pagination,
                Messages = asistencias.Messages
            };
            return Ok(response);
        }

        /// <summary>
        /// Recupera una lista de asistencias utilizando Dapper y las mapea a DTOs utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de asistencias utilizando Dapper para la consulta de datos y mapearlos automáticamente a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<AsistenciaDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper/dapper")]
        public async Task<IActionResult> GetAllAsistenciasDapperDtoMapper()
        {
            var asistencias = await _asistenciaService.GetAllAsistenciasDapperAsync();
            var asistenciaDtos = _mapper.Map<IEnumerable<AsistenciaDto>>(asistencias);

            var response = new ApiResponse<IEnumerable<AsistenciaDto>>(asistenciaDtos);

            return Ok(response);
        }

        /// <summary>
        /// Recupera una asistencia por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una asistencia específica utilizando su ID. Se realiza la validación del ID antes de la recuperación.
        /// </remarks>
        /// <param name="id"> Id de la asistencia buscada</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<AsistenciaDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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

        /// <summary>
        /// Registra una nueva asistencia utilizando un DTO y mapeo con AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite registrar una nueva asistencia utilizando un objeto DTO (Data Transfer Object) y mapeándolo automáticamente a la entidad correspondiente utilizando la biblioteca AutoMapper. Además, se realiza la validación del DTO antes de la inserción, 
        /// en el cual se tienen que cumplir las siguientes reglas: la Fecha de Asistencia es obligatoria, las id de usuario y horario tiene que ser validas y mayores a 1,
        /// , para registrar un usuario a la asistencia el usuario debe estar activo (IsActive = 1), el usuario no se puede registrar dos veces a la misma clase en el mismo horario,
        /// se verifica si la clase ya excedio la capacidad maxima, verifica si la membresia del usuario esta activa osea que si es valida comparando la fecha de la asistencia con la fecha de vencimiento de la membresia del usuario,
        /// verifica si en la membresia tiene clases y cuando lo registra lo pone en default como presente.
        /// </remarks>
        /// <param name="registrarAsistenciaDto">Registrar asistencia</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Asistencium>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
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

        /// <summary>
        /// Actualiza una asistencia existente utilizando un DTO y mapeo con AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite actualizar una asistencia existente utilizando un objeto DTO (Data Transfer Object) y mapeándolo automáticamente a la entidad correspondiente utilizando la biblioteca AutoMapper. Además, se realiza la validación del DTO antes de la actualización.
        /// </remarks>
        /// <param name="asistenciaDto">Actualizar asistencia</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Asistencium>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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

        /// <summary>
        /// Elimina una asistencia por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar una asistencia específica utilizando su ID.
        /// </remarks>
        /// <param name="id">Id de la asistencia a borrar</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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

        /// <summary>
        /// Recupera las asistencias por clase, horario y fecha.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener las asistencias registradas para una clase específica en un horario y fecha determinados.
        /// </remarks>
        /// <param name="claseId">Id de la clase</param>
        /// <param name="horario">Id de el horario</param>
        /// <param name="fecha">Fecha en el siguiente formato (YY/MM/DD)</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<Asistencium>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("GetAsistenciaByClase/{claseId}/{horario}/{fecha}")]
        public async Task<IActionResult> GetAsistenciaByClase(int claseId, int horario, DateOnly fecha)
        {
            var asistencias = await _asistenciaService.GetAsistenciaByClaseAsync(claseId, fecha);

            var response = new ApiResponse<IEnumerable<Asistencium>>(asistencias);

            return Ok(response);
        }

        /// <summary>
        /// Verifica si un usuario ya registró su asistencia para un horario y fecha específicos.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite verificar si un usuario ya ha registrado su asistencia para un horario y fecha específicos.
        /// </remarks>
        /// <param name="usuarioId">Id del usuario</param>
        /// <param name="horarioId">Id del horario</param>
        /// <param name="fecha">Fecha en el siguiente formato (YY/MM/DD)</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("UsuarioYaRegistroAsistencia/{usuarioId}/{horarioId}/{fecha}")]
        public async Task<IActionResult> UsuarioYaRegistroAsistencia(int usuarioId, int horarioId, DateOnly fecha)
        {
            var asistencia = await _asistenciaService.UsuarioYaRegistroAsistenciaAsync(usuarioId, horarioId, fecha);

            var response = new ApiResponse<bool>(asistencia);

            return Ok(response);
        }

        /// <summary>
        /// Recupera las asistencias por ID de usuario.
        /// </summary>
        /// <remarks>   
        /// Este endpoint permite obtener las asistencias registradas para un usuario específico utilizando su ID.
        /// </remarks>
        /// <param name="usuarioId">Id del usuario</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<Asistencium>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("GetAsistenciaByIdUsuario/{usuarioId}")]
        public async Task<IActionResult> GetAsistenciaUsuario(int usuarioId)
        {
            var asistencia = await _asistenciaService.GetAsistenciaByIdUsuarioAsync(usuarioId);

            var response = new ApiResponse<IEnumerable<Asistencium>>(asistencia);

            return Ok(response);
        }

        /// <summary>
        /// Recupera la asistencia con todos los datos relevevantes utilizando Dapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener las asistencias con todos los datos relevantes como ser el nombre del usuario, que dia asistio y en que horario, que clase y su fecha de asistencia utilizando Dapper para la consulta de datos.
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<AsistenciaCompletaResponse>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dapper/1")]
        public async Task<IActionResult> GetAsistenciaCompleta()
        {
            var asistencias = await _asistenciaService.GetAsistenciaCompleta();

            var response = new ApiResponse<IEnumerable<AsistenciaCompletaResponse>>(asistencias);

            return Ok(response);
        }

        #endregion
    }
}