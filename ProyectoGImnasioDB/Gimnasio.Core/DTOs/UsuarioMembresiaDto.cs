
using Gimnasio.Core.Entities;

namespace Gimnasio.Core.DTOs;
public class UsuarioMembresiaDto
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int MembresiaId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public int? ClasesRestantes { get; set; }

    public string? Estado { get; set; } =  "Activa";

    public decimal PrecioPagado { get; set; }

    public string? MetodoPago { get; set; } = null!;

    public virtual Membresia? Membresia { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; } = null!;
}