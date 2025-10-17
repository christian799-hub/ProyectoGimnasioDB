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

namespace Gimnasio.Api.Controllers
{
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

        #region  CRUD SIN DTO

        [HttpGet]
        public async Task<IActionResult> GetClases()
        {
            var clases = await _clasesService.GetAllClasesAsync();
            return Ok(clases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClaseById(int id)
        {
            var clase = await _clasesService.GetClaseByIdAsync(id);
            return Ok(clase);
        }

       [HttpPost]
        public async Task<IActionResult> InsertarClase(Clase clase)
        {
            await _clasesService.InsertClase(clase);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClase(Clase clase)
        {
            await _clasesService.UpdateClaseAsync(clase);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClase(int id)
        {
            var result = await _clasesService.GetClaseByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            await _clasesService.DeleteClaseAsync(result);
            return NoContent();
        }

        #endregion

        #region CRUD CON DTO

        [HttpGet("dto")]
        public async Task<IActionResult> GetClasesDto()
        {
            var clases = await _clasesService.GetAllClasesAsync();
            var clasesDto = clases.Select(c => new ClaseDto
            {
                Id = c.Id,
                Descripcion = c.Descripcion,
                InstructorId = c.InstructorId,
                CapacidadMaxima = c.CapacidadMaxima
            });

            return Ok(clasesDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetClaseByIdDto(int id)
        {
            var clase = await _clasesService.GetClaseByIdAsync(id);
            if (clase == null)
            {
                return NotFound();
            }

            var claseDto = new
            {
                clase.Id,
                clase.Descripcion,
                clase.InstructorId,
                clase.CapacidadMaxima,
                clase.DuracionMinutos,
                clase.Nivel,
                clase.IsActive,
                Instructor = clase.Instructor == null ? null : new
                {
                    clase.Instructor.Id,
                    clase.Instructor.Nombre,
                    clase.Instructor.Especialidad
                },
                Horarios = clase.Horarios ? .Select(h => new
                {
                    h.Id,
                    h.DiaSemana,
                    h.HoraInicio,
                    h.HoraFin
                }).ToList()
            };

            return Ok(claseDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertarClaseDto(ClaseDto claseDto)
        {
            var clase = new Clase
            {
                Descripcion = claseDto.Descripcion,
                InstructorId = claseDto.InstructorId,
                CapacidadMaxima = claseDto.CapacidadMaxima
            };

            await _clasesService.InsertClase(clase);
            return Ok(clase);
        }

        [HttpPut("dto")]
        public async Task<IActionResult> UpdateClaseDto(ClaseDto claseDto)
        {
            var clase = await _clasesService.GetClaseByIdAsync(claseDto.Id);

            if (clase == null)
            {
                return NotFound();
            }

            clase.Descripcion = claseDto.Descripcion;
            clase.InstructorId = claseDto.InstructorId;
            clase.CapacidadMaxima = claseDto.CapacidadMaxima;

            await _clasesService.UpdateClaseAsync(clase);
            return Ok();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteClaseDto(int id)
        {
            var clase = await _clasesService.GetClaseByIdAsync(id);
            if (clase == null)
            {
                return NotFound();
            }
            await _clasesService.DeleteClaseAsync(clase);
            return NoContent();
        }

        #region CRUD CON AUTOMAPPER


        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetClasesDtoMapper()
        {
            var clases = await _clasesService.GetAllClasesAsync();
            var clasesDto = _mapper.Map<IEnumerable<ClaseDto>>(clases);

            var response = new ApiResponse<IEnumerable<ClaseDto>>(clasesDto);

            return Ok(response);
        }

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

        #endregion

        #region Metodos Adicionales
        [HttpGet("instructor/{instructorId}")]
        public async Task<IActionResult> GetClaseByInstructor(int instructorId)
        {
            var clases = await _clasesService.GetClaseByInstructorAsync(instructorId);

            var response = new ApiResponse<IEnumerable<Clase>>(clases);

            return Ok(response);
        }
        #endregion
    }
}