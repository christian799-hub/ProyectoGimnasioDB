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

namespace Gimnasio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembresiaController : ControllerBase
    {
        private readonly IMembresiaService _membresiaService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
    
    public MembresiaController(IMembresiaService membresiaService, IMapper mapper, IValidationService validationService)
    {
        _mapper = mapper;
        _membresiaService = membresiaService;
        _validationService = validationService;
    }

    #region CRUD AUTOMAPPER

    /// <summary>
    /// Recupera una lista de membresias y las mapea a DTOs utilizando AutoMapper.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite obtener una lista de membresias con soporte para paginación y mapeo automático a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
    /// </remarks>
    /// <param name="membresiaQueryFilter">Los filtros se aplicacn al recuperar las clases como la paginacion y busqueda, si no se envia los paramtetros se retornan todos los registros</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<MembresiaDto>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpGet("dto/mapper")]
    public async Task<IActionResult> GetMembresiasDtoMapper([FromQuery] MembresiaQueryFilter membresiaQueryFilter)
    {
        var membresias = await _membresiaService.GetAllMembresiasAsync(membresiaQueryFilter);
        var membresiasDto = _mapper.Map<IEnumerable<MembresiaDto>>(membresias.Pagination);
        
        var pagination = new Pagination
        {
            TotalCount = membresias.Pagination.TotalCount,
            PageSize = membresias.Pagination.PageSize,
            CurrentPage = membresias.Pagination.CurrentPage,
            TotalPages = membresias.Pagination.TotalPages,
            HasNextPage = membresias.Pagination.HasNextPage,
            HasPreviousPage = membresias.Pagination.HasPreviousPage
        };

        var response = new ApiResponse<IEnumerable<MembresiaDto>>(membresiasDto)
        {
            Pagination = pagination,
            Messages = membresias.Messages
        };
        return Ok(response);
    }

    /// <summary>
    /// Recupera una lista de membresias utilizando Dapper y las mapea a DTOs utilizando AutoMapper.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite obtener una lista de membresias utilizando Dapper para la consulta de datos y mapeándolas automáticamente a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
    /// </remarks>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<MembresiaDto>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpGet("dto/mapper/dapper")]
    public async Task<IActionResult> GetMembresiasDtoMapperDapper()
    {
        var membresias = await _membresiaService.GetAllClaseDapperAsync();
        var membresiasDto = _mapper.Map<IEnumerable<MembresiaDto>>(membresias);

        var response = new ApiResponse<IEnumerable<MembresiaDto>>(membresiasDto);
        return Ok(response);
    }

    /// <summary>
    /// Recupera una membresia por su ID y lo mapea a un DTO utilizando AutoMapper.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite obtener una membresia específica por su ID y mapearlo automáticamente a un objeto DTO (Data Transfer Object) utilizando la biblioteca AutoMapper.
    /// </remarks>
    /// <param name="id">Id de la membresia</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<MembresiaDto>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpGet("dto/mapper/{id}")]
    public async Task<IActionResult> GetMembresiaByIdDtoMapper(int id)
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

        var membresia = await _membresiaService.GetMembresiaByIdAsync(id);
        if (membresia == null)
        {
            return NotFound();
        }

        var membresiaDto = _mapper.Map<MembresiaDto>(membresia);

        var response = new ApiResponse<MembresiaDto>(membresiaDto);

        return Ok(response);
    }

    /// <summary>
    /// Crea una nueva membresia a partir de un DTO utilizando AutoMapper.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite crear una nueva membresia a partir de un objeto DTO (Data Transfer Object) utilizando la biblioteca AutoMapper para mapear los datos al modelo de entidad correspondiente, 
    /// con las siguientes reglas: el tipo de membresía es obligatorio, el tipo de membresía no puede exceder los 50 caracteres, el precio y duracion tienen que ser mayores a 0,
    /// las clases restantes no puede ser negativos.
    /// </remarks>
    /// <param name="membresiaDto">Insertar membresia</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Membresia>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost("dto/mapper")]
    public async Task<IActionResult> InsertarMembresiaDtoMapper(MembresiaDto membresiaDto)
    {

        var validationResult = await _validationService.ValidateAsync(membresiaDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors });
            }
        
        var membresia = _mapper.Map<Membresia>(membresiaDto);
        await _membresiaService.InsertMembresia(membresia);

        var response = new ApiResponse<Membresia>(membresia);

        return Ok(response);
    }

    /// <summary>
    /// Actualiza una membresia existente utilizando un DTO y AutoMapper.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite actualizar una membresia existente utilizando un objeto DTO (Data Transfer Object) y mapeándolo automáticamente a la entidad correspondiente utilizando la biblioteca AutoMapper.
    /// </remarks>
    /// <param name="membresiaDto">Actualizar membresia</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<MembresiaDto>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpPut("dto/mapper")]
    public async Task<IActionResult> UpdateMembresiaDtoMapper(MembresiaDto membresiaDto)
    {
        var membresia = await _membresiaService.GetMembresiaByIdAsync(membresiaDto.Id ?? 0); //verificacion de uso  de null si no existe el id entonces usa 0
        if (membresia == null)
        {
            return NotFound();
        }

        _mapper.Map(membresiaDto, membresia);

        await _membresiaService.UpdateMembresiaAsync(membresia);

        var response = new ApiResponse<MembresiaDto>(membresiaDto);

        return Ok(response);
    }   

    /// <summary>
    /// Elimina una membresia por su ID.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite eliminar una membresia específica por su ID.
    /// </remarks>
    /// <param name="id">Id de la membresia a eliminar</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpDelete("dto/mapper/{id}")]
    public async Task<IActionResult> DeleteMembresiaDtoMapper(int id)
    {
        var membresia = await _membresiaService.GetMembresiaByIdAsync(id);
        if (membresia == null)
        {
            return NotFound();
        }

        await _membresiaService.DeleteMembresiaAsync(membresia);
        return NoContent();
    }

    /// <summary>
    /// Recupera una lista de membresias ordenadas por precio utilizando Dapper.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite obtener una lista de membresias ordenadas por precio del mas caro al mas barato utilizando Dapper para la consulta de datos.
    /// </remarks>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<MembresiaOrdenPrecioResponse>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpGet("dapper/1")]
    public async Task<IActionResult> GetMembresiaOrdenPrecio()
    {
        var membresias = await _membresiaService.GetMembresiaOrdenPrecio();

        var response = new ApiResponse<IEnumerable<MembresiaOrdenPrecioResponse>>(membresias);

        return Ok(response);
    }

    #endregion

    }

}