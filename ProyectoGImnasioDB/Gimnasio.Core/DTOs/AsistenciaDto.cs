using Gimnasio.Core.Entities;

namespace Gimnasio.Core.DTOs;

public  class AsistenciaDto
{

    public int UsuarioId { get; set; }

    public int HorarioId { get; set; }

    public DateOnly FechaAsistencia { get; set; }

    public int? Id { get; set; }

    public string? Estado { get; set; }

    public virtual Horario? Horario { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; } = null!;
}
