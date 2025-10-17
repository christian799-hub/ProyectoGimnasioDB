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
        #region  CRUD SIN DTO

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuariosService.GetAllUsuariosAsync();
            return Ok(usuarios);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> InsertarUsuario(Usuario usuario)
        {
            await _usuariosService.InsertUsuario(usuario);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUsuario(Usuario usuario)
        {
            await _usuariosService.UpdateUsuarioAsync(usuario);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var result = await _usuariosService.GetUsuarioByIdAsync(id);

            result.IsActive = 0;
            await _usuariosService.UpdateUsuarioAsync(result);
            return NoContent();
        }

    #endregion CRUD SIN DTO

    #region Crud CON DTO
    
        [HttpGet("dto")]
        public async Task<IActionResult> GetUsuariosDto()
        {
            var usuarios = await _usuariosService.GetAllUsuariosAsync();
            var usuariosDto = usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Edad = u.Edad,
                Telefono = u.Telefono,
                Asistencia = u.Asistencia,
                UsuarioMembresia = u.UsuarioMembresia,
                IsActive = u.IsActive
            });
            return Ok(usuariosDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetUsuarioByIdDto(int id)
        {
            var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Edad = usuario.Edad,
                Telefono = usuario.Telefono,
                Asistencia = usuario.Asistencia,
                UsuarioMembresia = usuario.UsuarioMembresia,
                IsActive = usuario.IsActive
            };
            return Ok(usuarioDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertarUsuarioDto(UsuarioDto usuarioDto)
        {
            var usuario = new Usuario
            {
                Nombre = usuarioDto.Nombre,
                Edad = usuarioDto.Edad,
                Telefono = usuarioDto.Telefono,
                IsActive = 1
            };
            await _usuariosService.InsertUsuario(usuario);
            return Ok(usuario);
        }

        [HttpPut("dto")]
        public async Task<IActionResult> UpdateUsuarioDto(UsuarioDto usuarioDto)
        {
            var usuario = await _usuariosService.GetUsuarioByIdAsync(usuarioDto.Id);
            if (usuario == null)
            {
                return NotFound();
            }
            usuario.Nombre = usuarioDto.Nombre;
            usuario.Edad = usuarioDto.Edad;
            usuario.Telefono = usuarioDto.Telefono;
            usuario.IsActive = usuarioDto.IsActive;

            await _usuariosService.UpdateUsuarioAsync(usuario);
            return Ok(usuario);
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteUsuarioDto(int id)
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
    
    #region Con Mapper

    [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetUsuariosDtoMapper()
        {
            var usuarios = await _usuariosService.GetAllUsuariosAsync();
            var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);

            var response = new ApiResponse<IEnumerable<UsuarioDto>>(usuariosDto);

            return Ok(response);
        }

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

        [HttpGet("activos")]
        public async Task<IActionResult> GetUsuariosActivos()
        {
            var usuarios = (await _usuariosService.GetAllUsuariosAsync()).Where(u => u.IsActive == 1);

            var response = new ApiResponse<IEnumerable<Usuario>>(usuarios);

            return Ok(response);
        }
        

    #endregion 

    }
}