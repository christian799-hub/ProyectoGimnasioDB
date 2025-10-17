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

    #region  CRUD SIN DTO

    [HttpGet]
    public async Task<IActionResult> GetMembresias()
    {
        var membresias = await _membresiaService.GetAllMembresiasAsync();
        return Ok(membresias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMembresiaById(int id)
    {
        var membresia = await _membresiaService.GetMembresiaByIdAsync(id);
        return Ok(membresia);
    }

    [HttpPost]
    public async Task<IActionResult> InsertarMembresia(Membresia membresia)
    {
        await _membresiaService.InsertMembresia(membresia);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMembresia(Membresia membresia)
    {
        await _membresiaService.UpdateMembresiaAsync(membresia);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMembresia(int id)
    {
        var result = await _membresiaService.GetMembresiaByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        await _membresiaService.DeleteMembresiaAsync(result);
        return NoContent();
    }

    #endregion

    #region CRUD CON DTO

    [HttpGet("dto")]
    public async Task<IActionResult> GetMembresiasDto()
    {
        var membresias = await _membresiaService.GetAllMembresiasAsync();
        var membresiasDto = membresias.Select(m => new MembresiaDto
        {
            Id = m.Id,
            Descripcion = m.Descripcion,
            Precio = m.Precio,
            ClasesIncluidas = m.ClasesIncluidas,
            DuracionDias = m.DuracionDias
            
        });

        return Ok(membresiasDto);
    }

    [HttpGet("dto/{id}")]
    public async Task<IActionResult> GetMembresiaByIdDto(int id)
    {
        var membresia = await _membresiaService.GetMembresiaByIdAsync(id);
        if (membresia == null)
        {
            return NotFound();
        }

        var membresiaDto = new MembresiaDto
        {
            Id = membresia.Id,
            Descripcion = membresia.Descripcion,
            Precio = membresia.Precio,
            ClasesIncluidas = membresia.ClasesIncluidas,
            DuracionDias = membresia.DuracionDias
        };

        return Ok(membresiaDto);
    }

    [HttpPost("dto")]
    public async Task<IActionResult> InsertarMembresiaDto(MembresiaDto membresiaDto)
    {
        var membresia = new Membresia
        {
            Descripcion = membresiaDto.Descripcion,
            Precio = membresiaDto.Precio,
            ClasesIncluidas = membresiaDto.ClasesIncluidas,
            DuracionDias = membresiaDto.DuracionDias
        };

        await _membresiaService.InsertMembresia(membresia);
        return Ok();
    }

    [HttpPut("dto")]
    public async Task<IActionResult> UpdateMembresiaDto(MembresiaDto membresiaDto)
    {
        var membresia = await _membresiaService.GetMembresiaByIdAsync(membresiaDto.Id ?? 0); //verificacion de uso  de null si no existe el id entonces usa 0
        if (membresia == null)
        {
            return NotFound();
        }

        membresia.Descripcion = membresiaDto.Descripcion;
        membresia.Precio = membresiaDto.Precio;
        membresia.ClasesIncluidas = membresiaDto.ClasesIncluidas;
        membresia.DuracionDias = membresiaDto.DuracionDias;

        await _membresiaService.UpdateMembresiaAsync(membresia);
        return Ok();
    }

    [HttpDelete("dto/{id}")]
    public async Task<IActionResult> DeleteMembresiaDto(int id)
    {
        var membresia = await _membresiaService.GetMembresiaByIdAsync(id);
        if (membresia == null)
        {
            return NotFound();
        }

        await _membresiaService.DeleteMembresiaAsync(membresia);
        return NoContent();
    }
    #endregion 

    #region CRUD AUTOMAPPER

    [HttpGet("dto/mapper")]
    public async Task<IActionResult> GetMembresiasDtoMapper()
    {
        var membresias = await _membresiaService.GetAllMembresiasAsync();
        var membresiasDto = _mapper.Map<IEnumerable<MembresiaDto>>(membresias);
        return Ok(membresiasDto);
    }

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

        var response = new ApiResponse<MembresiaDto>(membresiaDto);

        return Ok(response);
    }

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

    #endregion

    }

}