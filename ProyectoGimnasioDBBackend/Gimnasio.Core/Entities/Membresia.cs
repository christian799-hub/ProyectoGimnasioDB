using System;
using System.Collections.Generic;

namespace Gimnasio.Core.Entities;

public partial class Membresia : BaseEntity
{
    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int DuracionDias { get; set; }

    public int ClasesIncluidas { get; set; }

    public ulong? IsActive { get; set; }

    public virtual ICollection<UsuarioMembresia> UsuarioMembresia { get; set; } = new List<UsuarioMembresia>();
}
