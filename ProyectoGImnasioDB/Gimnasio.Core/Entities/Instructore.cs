using System;
using System.Collections.Generic;

namespace Gimnasio.Core.Entities;

public partial class Instructore
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Especialidad { get; set; }

    public string? Telefono { get; set; }

    public ulong? IsActive { get; set; }

    public virtual ICollection<Clase> Clases { get; set; } = new List<Clase>();
}
