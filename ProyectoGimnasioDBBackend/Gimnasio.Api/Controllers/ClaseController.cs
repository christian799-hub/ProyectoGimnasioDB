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
using Gimnasio.Core.QueryFilters;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Gimnasio.Api.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaseController : ControllerBase
    {
        private readonly IClasesService _clasesService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public ClaseController(IClasesService clasesService, IMapper mapper, IValidationService validationService)
        {
            _mapper = mapper;
            _clasesService = clasesService;
            _validationService = validationService;
        }


        #region CRUD CON AUTOMAPPER
        /// <summary>
        /// Recupera una lista paginada de clases y las mapea a DTOs utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de clases con soporte para paginación y mapeo automático a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <param name="claseQueryFilter">Los filtros se aplicacn al recuperar las clases como la paginacion y busqueda, si no se envia los paramtetros se retornan todos los registros</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ClaseDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper")] 
        public async Task<IActionResult> GetClasesDtoMapper([FromQuery] ClasesQueryFilter claseQueryFilter)
        {
            var clases = await _clasesService.GetAllClasesAsync(claseQueryFilter);
            var clasesDto = _mapper.Map<IEnumerable<ClaseDto>>(clases.Pagination);

            var pagination = new Pagination
            {
                TotalCount = clases.Pagination.TotalCount,
                PageSize = clases.Pagination.PageSize,
                CurrentPage = clases.Pagination.CurrentPage,
                TotalPages = clases.Pagination.TotalPages,
                HasNextPage = clases.Pagination.HasNextPage,
                HasPreviousPage = clases.Pagination.HasPreviousPage
            };

            

            var response = new ApiResponse<IEnumerable<ClaseDto>>(clasesDto)
            {
                Pagination = pagination,
                Messages = clases.Messages
            };
            return Ok(response);
        }


        /// <summary>
        /// Recupera todas las clases utilizando Dapper y las mapea a DTOs utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener todas las clases utilizando Dapper y mapearlas automáticamente a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ClaseDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper/dapper")]
        public async Task<IActionResult> GetAllClaseDapperDtoMapper()
        {
            var clases = await _clasesService.GetAllClaseDapperAsync();
            var clasesDto = _mapper.Map<IEnumerable<ClaseDto>>(clases);

            var response = new ApiResponse<IEnumerable<ClaseDto>>(clasesDto);

            return Ok(response);
        }

        /// <summary>
        /// Recupera una clase por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una clase específica por su ID y mapearla automáticamente a un objeto DTO (Data Transfer Object) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <param name="id">Id de la clase</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ClaseDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetClaseByIdDtoMapper(int id)
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

            var clase = await _clasesService.GetClaseByIdAsync(id);
            if (clase == null)
            {
                return NotFound();
            }

            var claseDto = _mapper.Map<ClaseDto>(clase);

            var response = new ApiResponse<ClaseDto>(claseDto);

            return Ok(response);
        }

        /// <summary>
        /// Inserta una nueva clase utilizando un DTO y AutoMapper.
        /// </summary>
        /// <remarks>
        /// /// Este endpoint permite insertar una nueva clase en el sistema utilizando un objeto DTO (Data Transfer Object) y mapeándolo automáticamente a la entidad correspondiente utilizando la biblioteca AutoMapper
        /// , al insertar el nombre de la clase es obligatorio, el nombre de la clase no puede exceder los 100 caracteres, el InstructorId no debe ser menor a -1, la capacidad máxima debe ser mayor que 40 ni menor a 10,
        /// , la duración en minutos debe ser mayor que 0 si se proporciona.
        /// </remarks>
        /// <param name="claseDto">Insertar clase</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Clase>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertarClaseDtoMapper(ClaseDto claseDto)
        {

             var validationResult = await _validationService.ValidateAsync(claseDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors });
            }

            var clase = _mapper.Map<Clase>(claseDto);
            await _clasesService.InsertClase(clase);

            var response = new ApiResponse<Clase>(clase);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza una clase existente utilizando un DTO y AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite actualizar una clase existente en el sistema utilizando un objeto DTO (Data Transfer Object) y mapeándolo automáticamente a la entidad correspondiente utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <param name="claseDto">Actulizar clase</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Clase>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("dto/mapper")]
        public async Task<IActionResult> UpdateClaseDtoMapper(ClaseDto claseDto)
        {
            var clase = await _clasesService.GetClaseByIdAsync(claseDto.Id);

            if (clase == null)
            {
                return NotFound();
            }

            _mapper.Map(claseDto, clase);

            await _clasesService.UpdateClaseAsync(clase);

            var response = new ApiResponse<Clase>(clase);

            return Ok(response);
        }

        /// <summary>
        /// Elimina una clase por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar una clase específica por su ID.
        /// </remarks>
        /// <param name="id">Id de la clase a borrar</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteClaseDtoMapper(int id)
        {
            var clase = await _clasesService.GetClaseByIdAsync(id);
            if (clase == null)
            {
                return NotFound();
            }
            await _clasesService.DeleteClaseAsync(clase);

            return NoContent();
        }

        #endregion

        /// <summary>
        /// Recupera una lista de clases impartidas por un instructor específico.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de clases que son impartidas por un instructor específico, identificado por su ID.
        /// </remarks>
        /// <param name="instructorId">Id del instructor</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<Clase>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        #region Metodos Adicionales
        [HttpGet("instructor/{instructorId}")]
        public async Task<IActionResult> GetClaseByInstructor(int instructorId)
        {
            var clases = await _clasesService.GetClaseByInstructorAsync(instructorId);

            var response = new ApiResponse<IEnumerable<Clase>>(clases);

            return Ok(response);
        }

        /// <summary>
        /// Recupera una lista de clases junto con la información de sus instructores utilizando Dapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de clases junto con la información de sus instructores utilizando Dapper para la consulta de datos.
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ClaseInstructorResponse>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dapper/1")]
        public async Task<IActionResult> GetClaseInstructor()
        {
            var clases = await _clasesService.GetClaseInstructor();

            var response = new ApiResponse<IEnumerable<ClaseInstructorResponse>>(clases);

            return Ok(response);
        }
        #endregion
    }
}