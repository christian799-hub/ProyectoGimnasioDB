using FluentValidation;
using Gimnasio.Core.DTOs;

namespace Gimnasio.Infrastructure.Validators;
public class HorarioDtoValidator : AbstractValidator<HorarioDto>
{
    public HorarioDtoValidator()
    {
        RuleFor(x => x.DiaSemana)
            .NotEmpty().WithMessage("El día de la semana es obligatorio.")
            .MaximumLength(20).WithMessage("El día de la semana no puede exceder los 20 caracteres.");

        RuleFor(x => x.HoraInicio)
            .NotEmpty().WithMessage("La hora de inicio es obligatoria.");

        RuleFor(x => x.HoraFin)
            .NotEmpty().WithMessage("La hora de fin es obligatoria.")
            .GreaterThan(x => x.HoraInicio).WithMessage("La hora de fin debe ser mayor que la hora de inicio.");
    }
}