using System;
using System.Collections.Generic;

namespace Gimnasio.Core.Entities;

public partial class Clase : BaseEntity
{
    public string Descripcion { get; set; }

    public int InstructorId { get; set; }

    public int CapacidadMaxima { get; set; }

    public int? DuracionMinutos { get; set; }

    public string? Nivel { get; set; }

    public ulong? IsActive { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual Instructore Instructor { get; set; } = null!;
}
