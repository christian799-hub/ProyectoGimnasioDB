using FluentValidation;
using Gimnasio.Core.DTOs;

namespace Gimnasio.Infrastructure.Validators;
public class UsuarioMembresiaDtoValidator : AbstractValidator<UsuarioMembresiaDto>
{
    public UsuarioMembresiaDtoValidator()
    {
        RuleFor(x => x.UsuarioId)
            .GreaterThan(0).WithMessage("El UsuarioId debe ser mayor que 0.");
        
        RuleFor(x => x.MembresiaId)
            .GreaterThan(0).WithMessage("El MembresiaId debe ser mayor que 0.");

        RuleFor(x => x.FechaInicio)
            .NotEmpty().WithMessage("La Fecha de Inicio es obligatoria.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("La Fecha de Inicio no puede ser en el futuro.");

        RuleFor(x => x.FechaFin)
            .NotEmpty().WithMessage("La Fecha de Fin es obligatoria.")
            .GreaterThan(x => x.FechaInicio).WithMessage("La Fecha de Fin debe ser mayor que la Fecha de Inicio.");
        
    }
}