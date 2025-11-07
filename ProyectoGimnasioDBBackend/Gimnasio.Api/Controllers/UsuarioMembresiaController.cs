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
    public class UsuarioMembresiaController : ControllerBase
    {
        private readonly IUsuarioMembresiaService _usuarioMembresiasService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public UsuarioMembresiaController(IUsuarioMembresiaService usuarioMembresiasService, IMapper mapper, IValidationService validationService)
        {
            _usuarioMembresiasService = usuarioMembresiasService;
            _mapper = mapper;
            _validationService = validationService;
        }

        #region CRUD CON AUTOMAPPER

        /// <summary>
        /// Recupera una lista de UsuarioMembresias y las mapea a DTOs utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de UsuarioMembresias con soporte para paginación y mapeo automático a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <param name="UsuarioMembresiaQueryFilter">Los filtros se aplicacn al recuperar las clases como la paginacion y busqueda, si no se envia los paramtetros se retornan todos los registros</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UsuarioMembresiaDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetUsuarioMembresiasDtoMapper([FromQuery] UsuarioMembresiaQueryFilter UsuarioMembresiaQueryFilter)
        {
            var usuarioMembresias = await _usuarioMembresiasService.GetAllUsuarioMembresiasAsync(UsuarioMembresiaQueryFilter);
            var usuarioMembresiasDto = _mapper.Map<IEnumerable<UsuarioMembresiaDto>>(usuarioMembresias.Pagination);

            var pagination = new Pagination
            {
            TotalCount = usuarioMembresias.Pagination.TotalCount,
            PageSize = usuarioMembresias.Pagination.PageSize,
            CurrentPage = usuarioMembresias.Pagination.CurrentPage,
            TotalPages = usuarioMembresias.Pagination.TotalPages,
            HasNextPage = usuarioMembresias.Pagination.HasNextPage,
            HasPreviousPage = usuarioMembresias.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<UsuarioMembresiaDto>>(usuarioMembresiasDto)
            {
            Pagination = pagination,
            Messages = usuarioMembresias.Messages
            };
        return Ok(response);
        }

        /// <summary>
        /// Recupera una lista de UsuarioMembresias utilizando Dapper y las mapea a DTOs utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de UsuarioMembresias utilizando Dapper para la consulta de datos y mapearlos automáticamente a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UsuarioMembresiaDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper/dapper")]
        public async Task<IActionResult> GetUsuarioMembresiasDapperDtoMapper()
        {
            var usuarioMembresias = await _usuarioMembresiasService.GetAllUsuariosDapperAsync();
            var usuarioMembresiasDto = _mapper.Map<IEnumerable<UsuarioMembresiaDto>>(usuarioMembresias);

            var response = new ApiResponse<IEnumerable<UsuarioMembresiaDto>>(usuarioMembresiasDto);

            return Ok(response);
        }

        /// <summary>
        /// Recupera una UsuarioMembresia por su ID y lo mapea a un DTO utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una UsuarioMembresia específica por su ID y mapearlo automáticamente a un objeto DTO (Data Transfer Object) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <param name="id">Id del UsuarioMembresia</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UsuarioMembresiaDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetUsuarioMembresiaByIdDtoMapper(int id)
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

            var um = await _usuarioMembresiasService.GetUsuarioMembresiaByIdAsync(id);
            if (um == null)
            {
                return NotFound();
            }

            var usuarioMembresiaDto = _mapper.Map<UsuarioMembresiaDto>(um);

            var response = new ApiResponse<UsuarioMembresiaDto>(usuarioMembresiaDto);

            return Ok(response);
        }

        /// <summary>
        /// Inserta una nueva UsuarioMembresia a partir de un DTO utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear una nueva UsuarioMembresia a partir de un objeto DTO (Data Transfer Object) utilizando la biblioteca AutoMapper, 
        /// con las siguientes reglas: los usuarios y membresias no pueden ser 0 y tienen que existir, la fecha de inicio y fin es obligatoria, cada vez que un usuario se vincula con UsuarioMembresia el IsActive cambia a 1.
        /// </remarks>
        /// <param name="usuarioMembresiaDto">Insertar UsuarioMembresia</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UsuarioMembresiaDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertarUsuarioMembresiaDtoMapper(UsuarioMembresiaDto usuarioMembresiaDto)
        {
            try{
                var validationResult = await _validationService.ValidateAsync(usuarioMembresiaDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var usuarioMembresia = _mapper.Map<UsuarioMembresia>(usuarioMembresiaDto);
                await _usuarioMembresiasService.InsertUsuarioMembresia(usuarioMembresia);

                var response = new ApiResponse<UsuarioMembresia>(usuarioMembresia);

                return Ok(response);
            }
            catch(Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,err.Message);
            }
        }

        /// <summary>
        /// Actualiza una UsuarioMembresia existente a partir de un DTO utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite actualizar una UsuarioMembresia existente a partir de un objeto DTO (Data Transfer Object) utilizando la biblioteca AutoMapper,
        /// </remarks>
        /// <param name="usuarioMembresiaDto">Actualizar usuarioMembresia</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UsuarioMembresia>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("dto/mapper")]
        public async Task<IActionResult> UpdateUsuarioMembresiaDtoMapper(UsuarioMembresiaDto usuarioMembresiaDto)
        {
            
            var usuarioMembresia = await _usuarioMembresiasService.GetUsuarioMembresiaByIdAsync(usuarioMembresiaDto.Id);
            if (usuarioMembresia == null) return NotFound();

            _mapper.Map(usuarioMembresiaDto, usuarioMembresia);

            await _usuarioMembresiasService.UpdateUsuarioMembresiaAsync(usuarioMembresia);

            var response = new ApiResponse<UsuarioMembresia>(usuarioMembresia);

            return Ok(response);
        }

        /// <summary>
        /// Elimina una UsuarioMembresia por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar una UsuarioMembresia específica por su ID.
        /// </remarks>
        /// <param name="id">Id del usuarioMembresia a eliminar</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteUsuarioMembresiaDtoMapper(int id)
        {
            var result = await _usuarioMembresiasService.GetUsuarioMembresiaByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            await _usuarioMembresiasService.DeleteUsuarioMembresiaAsync(result);
            return NoContent();
        }

        #endregion

        #region Métodos adicionales

        /// <summary>
        /// Verifica si un usuario tiene una membresía activa.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite verificar si un usuario específico tiene una membresía activa.
        /// </remarks>
        /// <param name="usuarioId">Id del usuario</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("usuario/{usuarioId}/tiene-membresia-activa")]
        public async Task<IActionResult> UsuarioTieneMembresiaActiva(int usuarioId)
        {
            bool tieneMembresiaActiva = await _usuarioMembresiasService.UsuarioTieneMembresiaActivaAsync(usuarioId);

            var response = new ApiResponse<bool>(tieneMembresiaActiva);

            return Ok(new {response});
        }
        
        /// <summary>
        /// Recupera la membresía activa de un usuario por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener la membresía activa de un usuario específico por su ID.
        /// </remarks>
        /// <param name="usuarioId">Id usuario</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UsuarioMembresia>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("usuario/{usuarioId}/membresia-activa")]
        public async Task<IActionResult> GetMembresiaActivaByUsuario(int usuarioId)
        {
            var membresiaActiva = await _usuarioMembresiasService.GetMembresiaActivaPorUsuarioIdAsync(usuarioId);
            if (membresiaActiva == null)
            {
                return NotFound();
            }

            var response = new ApiResponse<UsuarioMembresia>(membresiaActiva);

            return Ok(response);
        }

        /// <summary>
        /// Recupera una lista de usuarios junto con sus membresías utilizando Dapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de usuarios junto con sus membresías utilizando Dapper para la consulta de datos.
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UsuarioYMembresiaResponse>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dapper/1")]
        public async Task<IActionResult> GetUsuarioYMembresia()
        {
            var usuarioYMembresia = await _usuarioMembresiasService.GetUsuarioYMembresia();

            var response = new ApiResponse<IEnumerable<UsuarioYMembresiaResponse>>(usuarioYMembresia);

            return Ok(response);
        } //descripcion y clases restantes no sirven corregir


        #endregion
    }
}