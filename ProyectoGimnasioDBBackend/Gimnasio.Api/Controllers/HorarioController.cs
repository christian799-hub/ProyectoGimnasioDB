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
        
        #region  CRUD SIN DTO

        [HttpGet]
        public async Task<IActionResult> GetHorarios()
        {
            var horarios = await _horariosService.GetAllHorariosAsync();
            return Ok(horarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHorarioById(int id)
        {
            var horario = await _horariosService.GetHorarioByIdAsync(id);
            return Ok(horario);
        }

        [HttpPost]
        public async Task<IActionResult> InsertarHorario(Horario horario)
        {
            await _horariosService.InsertHorario(horario);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateHorario(Horario horario)
        {
            await _horariosService.UpdateHorarioAsync(horario);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            var result = await _horariosService.GetHorarioByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            await _horariosService.DeleteHorarioAsync(result);
            return NoContent();
        }

        #endregion

        #region CRUD CON DTO
    [HttpGet("dto")]
    public async Task<IActionResult> GetHorariosDto()
    {
        var horarios = await _horariosService.GetAllHorariosAsync();
        var horariosDto = horarios.Select(h => new
        {
            h.Id,
            h.ClaseId,
            h.DiaSemana,
            h.HoraInicio,
            h.HoraFin,
            h.Sala,
            h.IsActive,
            Clase = h.Clase == null ? null : new
            {
                h.Clase.Id,
                h.Clase.Descripcion,
                h.Clase.InstructorId
            }
        });
        return Ok(horariosDto);
    }

    [HttpGet("dto/{id}")]
    public async Task<IActionResult> GetHorarioByIdDto(int id)
    {
        var horario = await _horariosService.GetHorarioByIdAsync(id);
        if (horario == null)
            {
                return NotFound();
            }
            
            var horarioDto = new
            {
                Id = horario.Id,
                ClaseId = horario.ClaseId,
                DiaSemana = horario.DiaSemana,
                HoraInicio = horario.HoraInicio,
                HoraFin = horario.HoraFin,
                Sala = horario.Sala,
                IsActive = horario.IsActive,
                Clase = horario.Clase == null ? null : new
                {
                    horario.Clase.Id,
                    horario.Clase.Descripcion,
                    horario.Clase.InstructorId
                }      
            };
    
            return Ok(horarioDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertarHorarioDto(HorarioDto horarioDto)
        {
            var horario = new Horario
            {
                ClaseId = horarioDto.ClaseId,
                DiaSemana = horarioDto.DiaSemana,
                HoraInicio = horarioDto.HoraInicio,
                HoraFin = horarioDto.HoraFin
            };
            await _horariosService.InsertHorario(horario);
            return Ok(horario);
        }

        [HttpPut("dto")]
        public async Task<IActionResult> UpdateHorarioDto(HorarioDto horarioDto)
        {
            var horario =  await _horariosService.GetHorarioByIdAsync(horarioDto.Id);

            if (horario == null)
            {
                return NotFound();
            }

            horario.ClaseId = horarioDto.ClaseId;
            horario.DiaSemana = horarioDto.DiaSemana;
            horario.HoraInicio = horarioDto.HoraInicio;
            horario.HoraFin = horarioDto.HoraFin;

            await _horariosService.UpdateHorarioAsync(horario);
            return Ok();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteHorarioDto(int id)
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

        #region CRUD CON AUTOMAPPER

        [HttpGet("dto/mapper")]
    public async Task<IActionResult> GetHorariosDtoMapper()
    {
        var horarios = await _horariosService.GetAllHorariosAsync();
        var horariosDto = _mapper.Map<IEnumerable<HorarioDto>>(horarios);

        var response = new ApiResponse<IEnumerable<HorarioDto>>(horariosDto);
        
        return Ok(response);
    }

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

        [HttpGet("clase/{claseId}")]
        public async Task<IActionResult> GetHorariosByClaseId(int claseId)
        {
            var horarios = await _horariosService.GetHorariosByClaseAsync(claseId);

            var response = new ApiResponse<IEnumerable<Horario>>(horarios);

            return Ok(response);
        }

        #endregion
    }
}