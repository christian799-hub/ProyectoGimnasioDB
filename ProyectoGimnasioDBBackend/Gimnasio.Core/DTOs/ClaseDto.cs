using System;
using System.Collections.Generic;
using Gimnasio.Core.Entities;

namespace Gimnasio.Core.DTOs;

public class ClaseDto
{
    public int Id { get; set; }

    public string Descripcion { get; set; }

    public int InstructorId { get; set; }

    public int CapacidadMaxima { get; set; }

    public int? DuracionMinutos { get; set; }

    public string? Nivel { get; set; }

    public ulong? IsActive { get; set; }

    public virtual ICollection<Horario>? Horarios { get; set; } = new List<Horario>();

}
