using FluentValidation;
using Gimnasio.Core.DTOs;

namespace Gimnasio.Infrastructure.Validators;
public class MembresiaDtoValidator : AbstractValidator<MembresiaDto>
{
    public MembresiaDtoValidator()
    {
        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("El tipo de membresía es obligatorio.")
            .MaximumLength(50).WithMessage("El tipo de membresía no puede exceder los 50 caracteres.");

        RuleFor(x => x.Precio)
            .GreaterThan(0).WithMessage("El precio debe ser mayor que 0.");

        RuleFor(x => x.DuracionDias)
            .GreaterThan(0).WithMessage("La duración en dias debe ser mayor que 0.");
    
        RuleFor(x => x.ClasesIncluidas)
            .GreaterThanOrEqualTo(0).WithMessage("Las clases incluidas no pueden ser negativas.");
    }
}