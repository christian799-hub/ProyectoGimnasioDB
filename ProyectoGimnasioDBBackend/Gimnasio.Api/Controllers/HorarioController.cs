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
    public class HorarioController : ControllerBase
    {
        private readonly IHorarioService _horariosService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public HorarioController(IHorarioService horariosService, IMapper mapper, IValidationService validationService)
        {
            _mapper = mapper;
            _horariosService = horariosService;
            _validationService = validationService;
        }
        
        #region CRUD CON AUTOMAPPER

    /// <summary>
    /// Recupera una lista de horarios y las mapea a DTOs utilizando AutoMapper.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite obtener una lista de horarios con soporte para paginación y mapeo automático a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
    /// </remarks>
    /// <param name="horarioQueryFilter">Los filtros se aplicacn al recuperar las clases como la paginacion y busqueda, si no se envia los paramtetros se retornan todos los registros</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<HorarioDto>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpGet("dto/mapper")]
    public async Task<IActionResult> GetHorariosDtoMapper([FromQuery] HorarioQueryFilter horarioQueryFilter)
    {
        var horarios = await _horariosService.GetAllHorariosAsync(horarioQueryFilter);
        var horariosDto = _mapper.Map<IEnumerable<HorarioDto>>(horarios.Pagination);
    
        var pagination = new Pagination
        {
            TotalCount = horarios.Pagination.TotalCount,
            PageSize = horarios.Pagination.PageSize,
            CurrentPage = horarios.Pagination.CurrentPage,
            TotalPages = horarios.Pagination.TotalPages,
            HasNextPage = horarios.Pagination.HasNextPage,
            HasPreviousPage = horarios.Pagination.HasPreviousPage
        };

        var response = new ApiResponse<IEnumerable<HorarioDto>>(horariosDto)
        {
            Pagination = pagination,
            Messages = horarios.Messages
        };
        return Ok(response);
    }

    /// <summary>
    /// Recupera una lista de horarios utilizando Dapper y las mapea a DTOs utilizando AutoMapper.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite obtener una lista de horarios utilizando Dapper para la consulta de datos y mapearlos automáticamente a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
    /// </remarks>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<HorarioDto>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpGet("dto/mapper/dapper")]
    public async Task<IActionResult> GetAllHorariosDapperDtoMapper()
    {
        var horarios = await _horariosService.GetAllClaseDapperAsync();
        var horariosDto = _mapper.Map<IEnumerable<HorarioDto>>(horarios);

        var response = new ApiResponse<IEnumerable<HorarioDto>>(horariosDto);

        return Ok(response);
    }

    /// <summary>
    /// Recupera un horario por su ID y lo mapea a un DTO utilizando AutoMapper.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite obtener un horario específico por su ID y mapearlo automáticamente a un objeto DTO (Data Transfer Object) utilizando la biblioteca AutoMapper.
    /// </remarks>
    /// <param name="id">Id del horario</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<HorarioDto>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpGet("dto/mapper/{id}")]
    public async Task<IActionResult> GetHorarioByIdDtoMapper(int id)
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

        var horario = await _horariosService.GetHorarioByIdAsync(id);
        if (horario == null)
            {
                return NotFound();
            }
            
            var horarioDto = _mapper.Map<HorarioDto>(horario);

            var response = new ApiResponse<HorarioDto>(horarioDto);
    
            return Ok(response);
        }

        /// <summary>
        /// Inserta un nuevo horario a partir de un DTO mapeado con AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite insertar un nuevo horario utilizando un objeto DTO (Data Transfer Object) que se mapea automáticamente a la entidad Horario utilizando la biblioteca AutoMapper, 
        /// con las siguientes reglas: El dia de la semana no puede exceder de 20 letras, la hora de inicio y fin es obligatoria, la hora de fin debe ser mayor que la hora de inicio,
        /// no se puede asignar un horario a una clase inactiva y no se puede asignar una clase inexistente.
        /// </remarks>
        /// <param name="horarioDto">Insertar horario</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Horario>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertarHorarioDtoMapper(HorarioDto horarioDto)
        {
            try{

                var validationResult = await _validationService.ValidateAsync(horarioDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var horario = _mapper.Map<Horario>(horarioDto);
                await _horariosService.InsertHorario(horario);

                var response = new ApiResponse<Horario>(horario);

                return Ok(response);
            }
            catch(Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,err.Message);
            }
            
        }

        /// <summary>
        /// Actualiza un horario existente utilizando un DTO mapeado con AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite actualizar un horario existente en el sistema utilizando un objeto DTO (Data Transfer Object) que se mapea automáticamente a la entidad Horario utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <param name="horarioDto">Actualizar horario</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Horario>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("dto/mapper")]
        public async Task<IActionResult> UpdateHorarioDtoMapper(HorarioDto horarioDto)
        {
            var horario =  await _horariosService.GetHorarioByIdAsync(horarioDto.Id);

            if (horario == null)
            {
                return NotFound();
            }

            _mapper.Map(horarioDto, horario);

            await _horariosService.UpdateHorarioAsync(horario);

            var response = new ApiResponse<Horario>(horario);

            return Ok(response);
        }

        /// <summary>
        /// Elimina un horario por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar un horario específico por su ID.
        /// </remarks>
        /// <param name="id">Id del horario a eliminar</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteHorarioDtoMapper(int id)
        {
            var horario = await _horariosService.GetHorarioByIdAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            await _horariosService.DeleteHorarioAsync(horario);
            return NoContent();
        }

        #endregion
        
        #region  Metodos Adicionales 

        /// <summary>
        /// Recupera una lista de horarios asociados a una clase específica por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de horarios que están asociados a una clase específica, identificada por su ID.
        /// </remarks>
        /// <param name="claseId">Id de la clase</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<Horario>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("clase/{claseId}")]
        public async Task<IActionResult> GetHorariosByClaseId(int claseId)
        {
            var horarios = await _horariosService.GetHorariosByClaseAsync(claseId);

            var response = new ApiResponse<IEnumerable<Horario>>(horarios);

            return Ok(response);
        }

        /// <summary>
        /// Recupera una lista de horarios con disponibilidad de clases.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de horarios que tienen las clases ordenadas por dia.
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<HorarioDisponibilidadResponse>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dapper/1")]
        public async Task<IActionResult> GetHorariosDisponibilidad()
        {
            var horarios = await _horariosService.GetHorariosDisponibilidadAsync();

            var response = new ApiResponse<IEnumerable<HorarioDisponibilidadResponse>>(horarios);

            return Ok(response);
        }

        #endregion
    }
}