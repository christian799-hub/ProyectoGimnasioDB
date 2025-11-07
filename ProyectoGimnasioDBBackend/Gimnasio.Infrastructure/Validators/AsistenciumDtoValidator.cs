using FluentValidation;
using Gimnasio.Core.DTOs;

namespace Gimnasio.Infrastructure.Validators;
public class AsistenciumDtoValidator : AbstractValidator<AsistenciaDto>
{
    public AsistenciumDtoValidator()
    {
        RuleFor(x => x.UsuarioId)
            .GreaterThan(0).WithMessage("El UsuarioId debe ser mayor que 0.");
        
        RuleFor(x => x.HorarioId)
            .GreaterThan(0).WithMessage("El HorarioId debe ser mayor que 0.");

        RuleFor(x => x.FechaAsistencia)
            .NotEmpty().WithMessage("La Fecha de Asistencia es obligatoria.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("La Fecha de Asistencia no puede ser en el futuro.");

    }
}