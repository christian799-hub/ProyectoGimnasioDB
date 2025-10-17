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

        #region  CRUD SIN DTO

        [HttpGet]
        public async Task<IActionResult> GetUsuarioMembresias()
        {
            var usuarioMembresias = await _usuarioMembresiasService.GetAllUsuarioMembresiasAsync();
            return Ok(usuarioMembresias);
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetUsuarioMembresiaById(int id)
        {
            var usuarioMembresia = await _usuarioMembresiasService.GetUsuarioMembresiaByIdAsync(id);
            return Ok(usuarioMembresia);
        }

        [HttpPost]
        public async Task<IActionResult> InsertarUsuarioMembresia(UsuarioMembresia usuarioMembresia)
        {
            await _usuarioMembresiasService.InsertUsuarioMembresia(usuarioMembresia);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsuarioMembresia(UsuarioMembresia usuarioMembresia)
        {
            await _usuarioMembresiasService.UpdateUsuarioMembresiaAsync(usuarioMembresia);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioMembresia(int id)
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

        #region CRUD CON DTO

        [HttpGet("dto")]
        public async Task<IActionResult> GetUsuarioMembresiasDto()
        {
            var usuarioMembresias = await _usuarioMembresiasService.GetAllUsuarioMembresiasAsync();
            var usuarioMembresiasDto = usuarioMembresias.Select(um => new 
            {
                Id = um.Id,
                UsuarioId = um.UsuarioId,
                MembresiaId = um.MembresiaId,
                FechaInicio = um.FechaInicio,
                FechaFin = um.FechaFin,
                Usuario = um.Usuario == null ? null : new  
                {
                    Id = um.Usuario.Id,
                    Nombre = um.Usuario.Nombre,
                    Edad = um.Usuario.Edad,
                    Telefono = um.Usuario.Telefono,
                    Asistencia = um.Usuario.Asistencia,
                    IsActive = um.Usuario.IsActive
                },
                Membresia = um.Membresia == null ? null : new
                {
                    Id = um.Membresia.Id,
                    Descripcion = um.Membresia.Descripcion,
                    Precio = um.Membresia.Precio,
                    DuracionDias = um.Membresia.DuracionDias
                }
            }).ToList();

            return Ok(usuarioMembresiasDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetUsuarioMembresiaByIdDto(int id)
        {
            var um = await _usuarioMembresiasService.GetUsuarioMembresiaByIdAsync(id);
            if (um == null)
            {
                return NotFound();
            }

            var usuarioMembresiaDto = new 
            {
                Id = um.Id,
                UsuarioId = um.UsuarioId,
                MembresiaId = um.MembresiaId,
                FechaInicio = um.FechaInicio,
                FechaFin = um.FechaFin,
                Usuario = um.Usuario == null ? null : new  
                {
                    Id = um.Usuario.Id,
                    Nombre = um.Usuario.Nombre,
                    Edad = um.Usuario.Edad,
                    Telefono = um.Usuario.Telefono,
                    Asistencia = um.Usuario.Asistencia,
                    IsActive = um.Usuario.IsActive
                },
                Membresia = um.Membresia == null ? null : new
                {
                    Id = um.Membresia.Id,
                    Descripcion = um.Membresia.Descripcion,
                    Precio = um.Membresia.Precio,
                    DuracionDias = um.Membresia.DuracionDias
                }
            };

            return Ok(usuarioMembresiaDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertarUsuarioMembresiaDto(UsuarioMembresiaDto usuarioMembresiaDto)
        {
            var usuarioMembresia = new UsuarioMembresia
            {
                UsuarioId = usuarioMembresiaDto.UsuarioId,
                MembresiaId = usuarioMembresiaDto.MembresiaId,
                FechaInicio = usuarioMembresiaDto.FechaInicio,
                FechaFin = usuarioMembresiaDto.FechaFin,
                ClasesRestantes = usuarioMembresiaDto.ClasesRestantes ?? 0,
                Estado = usuarioMembresiaDto.Estado ?? "Activa",
                MetodoPago = usuarioMembresiaDto.MetodoPago ?? "Efectivo",
                PrecioPagado = usuarioMembresiaDto.PrecioPagado
            };

            await _usuarioMembresiasService.InsertUsuarioMembresia(usuarioMembresia);
            return Ok(usuarioMembresia);
        }

        [HttpPut("dto")]
        public async Task<IActionResult> UpdateUsuarioMembresiaDto([FromBody] UsuarioMembresiaDto usuarioMembresiaDto)
        {
            
            var usuarioMembresia = await _usuarioMembresiasService.GetUsuarioMembresiaByIdAsync(usuarioMembresiaDto.Id);
            if (usuarioMembresia == null) return NotFound();

            usuarioMembresia.UsuarioId = usuarioMembresiaDto.UsuarioId;
            usuarioMembresia.MembresiaId = usuarioMembresiaDto.MembresiaId;
            usuarioMembresia.FechaInicio = usuarioMembresiaDto.FechaInicio;
            usuarioMembresia.FechaFin = usuarioMembresiaDto.FechaFin;


            await _usuarioMembresiasService.UpdateUsuarioMembresiaAsync(usuarioMembresia);
            return Ok(usuarioMembresia);
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteUsuarioMembresiaDto(int id)
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

        #region CRUD CON AUTOMAPPER

        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetUsuarioMembresiasDtoMapper()
        {
            var usuarioMembresias = await _usuarioMembresiasService.GetAllUsuarioMembresiasAsync();
            var usuarioMembresiasDto = _mapper.Map<IEnumerable<UsuarioMembresiaDto>>(usuarioMembresias);

            var response = new ApiResponse<IEnumerable<UsuarioMembresiaDto>>(usuarioMembresiasDto);

            return Ok(response);
        }

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

                var response = new ApiResponse<UsuarioMembresiaDto>(usuarioMembresiaDto);

                return Ok(response);
            }
            catch(Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,err.Message);
            }
        }

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

        [HttpGet("usuario/{usuarioId}/tiene-membresia-activa")]
        public async Task<IActionResult> UsuarioTieneMembresiaActiva(int usuarioId)
        {
            bool tieneMembresiaActiva = await _usuarioMembresiasService.UsuarioTieneMembresiaActivaAsync(usuarioId);

            var response = new ApiResponse<bool>(tieneMembresiaActiva);

            return Ok(new {response});
        }
        
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


        #endregion
    }
}