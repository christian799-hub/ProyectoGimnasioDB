using System;
using System.Collections.Generic;

namespace Gimnasio.Core.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Edad { get; set; }

    public string Telefono { get; set; } = null!;

    public ulong IsActive { get; set; }

    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    public virtual ICollection<UsuarioMembresia> UsuarioMembresia { get; set; } = new List<UsuarioMembresia>();


}
