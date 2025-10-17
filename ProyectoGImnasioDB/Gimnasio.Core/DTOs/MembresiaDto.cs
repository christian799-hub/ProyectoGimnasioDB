
using Gimnasio.Core.Entities;

namespace Gimnasio.Core.DTOs;

public class MembresiaDto
{
    public int? Id { get; set; }

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int DuracionDias { get; set; }

    public int ClasesIncluidas { get; set; }

    public ulong? IsActive { get; set; }

    public virtual ICollection<UsuarioMembresia>? UsuarioMembresia { get; set; } = new List<UsuarioMembresia>();
}