using Microsoft.AspNetCore.Mvc;
using Gimnasio.Infrastructure.Data;
using Gimnasio.Core.Interfaces;
using Gimnasio.Infrastructure.Repositories;
using Gimnasio.Core.Entities;
using Gimnasio.Core.DTOs;
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
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuariosService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public UsuariosController(IUsuarioService usuariosService, IMapper mapper, IValidationService validationService)
        {
            _usuariosService = usuariosService;
            _mapper = mapper;
            _validationService = validationService;

        }
    
    #region Con Mapper

        /// <summary>
        /// Recupera una lista de usuarios y las mapea a DTOs utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de usuarios y mapearlos automáticamente a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <param name="usuarioQueryFilter">Los filtros se aplicacn al recuperar las clases como la paginacion y busqueda, si no se envia los paramtetros se retornan todos los registros</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UsuarioDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetUsuariosDtoMapper([FromQuery] UsuarioQueryFilter usuarioQueryFilter)
        {
            var usuarios = await _usuariosService.GetAllUsuariosAsync(usuarioQueryFilter);
            var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios.Pagination);

            var pagination = new Pagination
            {
            TotalCount = usuarios.Pagination.TotalCount,
            PageSize = usuarios.Pagination.PageSize,
            CurrentPage = usuarios.Pagination.CurrentPage,
            TotalPages = usuarios.Pagination.TotalPages,
            HasNextPage = usuarios.Pagination.HasNextPage,
            HasPreviousPage = usuarios.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<UsuarioDto>>(usuariosDto)
            {
            Pagination = pagination,
            Messages = usuarios.Messages
            };
            return Ok(response);
        }

        /// <summary>
        /// Recupera una lista de usuarios utilizando Dapper y las mapea a DTOs utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de usuarios utilizando Dapper para la consulta de datos y mapearlos automáticamente a objetos DTO (Data Transfer Objects) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UsuarioDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper/dapper")] 
        public async Task<IActionResult> GetUsuariosDtoDapper()
        {
            var usuarios = await _usuariosService.GetAllUsuariosDapperAsync();
            var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);

            var response = new ApiResponse<IEnumerable<UsuarioDto>>(usuariosDto);

            return Ok(response);
        }

        /// <summary>
        /// Recupera un usuario por su ID y lo mapea a un DTO utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener un usuario específico por su ID y mapearlo automáticamente a un objeto DTO (Data Transfer Object) utilizando la biblioteca AutoMapper.
        /// </remarks>
        /// <param name="id">Id del usuario</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UsuarioDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetUsuarioByIdDtoMapper(int id)
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
            
            var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

            var response = new ApiResponse<UsuarioDto>(usuarioDto);

            return Ok(response);
        }

        /// <summary>
        /// Inserta un nuevo usuario a partir de un DTO utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// /// Este endpoint permite insertar un nuevo usuario en el sistema a partir de un objeto DTO (Data Transfer Object) utilizando la biblioteca AutoMapper para mapear los datos, 
        /// con las siguientes reglas: El nombre y el telefono son obligatorios, IsActive es solo 1 o 0, el rango de edad debe estar entre 10 y 100 años.
        /// </remarks>
        /// <param name="usuarioDto">Insertar Usuario</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Usuario>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertarUsuarioDtoMapper(UsuarioDto usuarioDto)
        {
            
            var validationResult = await _validationService.ValidateAsync(usuarioDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors });
            }

            var usuario = _mapper.Map<Usuario>(usuarioDto);
            await _usuariosService.InsertUsuario(usuario);

            var response = new ApiResponse<Usuario>(usuario);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza un usuario existente a partir de un DTO utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite actualizar un usuario existente en el sistema a partir de un objeto DTO (Data Transfer Object) utilizando la biblioteca AutoMapper para mapear los datos,
        /// </remarks>
        /// <param name="usuarioDto">Actualizar usuario</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Usuario>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("dto/mapper")]
        public async Task<IActionResult> UpdateUsuarioDtoMapper(UsuarioDto usuarioDto)
        {
            var usuario = await _usuariosService.GetUsuarioByIdAsync(usuarioDto.Id);
            if (usuario == null)
            {
                return NotFound();
            }
            _mapper.Map(usuarioDto, usuario);

            await _usuariosService.UpdateUsuarioAsync(usuario);

            var response = new ApiResponse<Usuario>(usuario);

            return Ok(response);
        }

        /// <summary>
        /// Elimina (desactiva) un usuario existente estableciendo su propiedad IsActive a 0.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar (desactivar) un usuario existente en el sistema estableciendo su propiedad IsActive a 0 en lugar de eliminar físicamente el registro de la base de datos.
        /// </remarks>
        /// <param name="id">Id Usuario a borrar</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteUsuarioDtoMapper(int id)
        {
            var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            usuario.IsActive = 0;
            await _usuariosService.UpdateUsuarioAsync(usuario);
            return NoContent();
        }

    #endregion

    #region Metodos Adicionales

    /// <summary>
        /// Recupera una lista de usuarios activos (IsActive = 1).
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de usuarios que están activos en el sistema, es decir, aquellos cuyo campo IsActive es igual a 1.
        /// </remarks>
        /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<Usuario>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpGet("activos")]
    public async Task<IActionResult> GetUsuariosActivos([FromQuery] UsuarioQueryFilter usuarioQueryFilter)
    {
        var usuarios = await _usuariosService.GetAllUsuariosAsync(usuarioQueryFilter);

        var usuariosActivos = usuarios.Pagination.Cast<Usuario>().Where(u => u.IsActive == 1).ToList();

        var response = new ApiResponse<IEnumerable<Usuario>>(usuariosActivos)
        {
            Messages = usuarios.Messages
        };

        return Ok(response);
    }

    /// <summary>
    /// Recupera una lista de usuarios y su cantidad de asistencias utilizando Dapper.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite obtener una lista de usuarios con su cantidad de asistencias utilizando Dapper para la consulta de datos.
    /// </remarks>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UsuarioAsistenciaResponse>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpGet("dapper/1")]
    public async Task<IActionResult> GetAsistenciasUsuarios()
    {
        var usuarios = await _usuariosService.GetAsistenciasUsuarios();

        var response = new ApiResponse<IEnumerable<UsuarioAsistenciaResponse>>(usuarios);

        
        return Ok(response);
    }
        

    #endregion 


    }
}