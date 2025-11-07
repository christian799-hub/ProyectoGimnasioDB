
using Gimnasio.Core.Entities;

namespace Gimnasio.Core.DTOs;
public class UsuarioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    
    public int Edad { get; set; }
    public string Telefono { get; set; }
    public DateOnly FechaNacimiento { get; set; }
    public ulong IsActive { get; set; }
    public virtual ICollection<Asistencium>? Asistencia { get; set; } = new List<Asistencium>();

    public virtual ICollection<UsuarioMembresia>? UsuarioMembresia { get; set; } = new List<UsuarioMembresia>();
}