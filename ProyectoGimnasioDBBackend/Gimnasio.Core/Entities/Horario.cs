using System;
using System.Collections.Generic;

namespace Gimnasio.Core.Entities;

public partial class Horario : BaseEntity
{

    public int ClaseId { get; set; }

    public string DiaSemana { get; set; } = null!;

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public string? Sala { get; set; }

    public ulong? IsActive { get; set; }

    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    public virtual Clase Clase { get; set; } = null!;
}
