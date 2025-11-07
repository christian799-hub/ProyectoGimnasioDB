using System;
using System.Collections.Generic;

namespace Gimnasio.Core.Entities;

public partial class Asistencium : BaseEntity
{
    public int UsuarioId { get; set; }

    public int HorarioId { get; set; }

    public DateOnly FechaAsistencia { get; set; }

    public string? Estado { get; set; }

    public virtual Horario Horario { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
