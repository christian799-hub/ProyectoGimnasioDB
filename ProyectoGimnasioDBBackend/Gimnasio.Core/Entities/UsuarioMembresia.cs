using System;
using System.Collections.Generic;

namespace Gimnasio.Core.Entities;

public partial class UsuarioMembresia : BaseEntity
{
    public int UsuarioId { get; set; }

    public int MembresiaId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public int? ClasesRestantes { get; set; }

    public string Estado { get; set; } = "Activa";

    public decimal PrecioPagado { get; set; }

    public string MetodoPago { get; set; } = null!;

    public virtual Membresia Membresia { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
