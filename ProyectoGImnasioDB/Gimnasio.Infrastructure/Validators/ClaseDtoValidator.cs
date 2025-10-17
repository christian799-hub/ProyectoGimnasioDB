using FluentValidation;
using Gimnasio.Core.DTOs;

namespace Gimnasio.Infrastructure.Validators;
public class ClaseDtoValidator : AbstractValidator<ClaseDto>
{
    public ClaseDtoValidator()
    {
        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("El nombre de la clase es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre de la clase no puede exceder los 100 caracteres.");
        
        RuleFor(x => x.InstructorId)
            .GreaterThan(0).WithMessage("El InstructorId no debe ser menor a -1.");

        RuleFor(x => x.CapacidadMaxima)
            .InclusiveBetween(10,40).WithMessage("La capacidad máxima debe ser mayor que 40 ni menor a 10.");

        RuleFor(x => x.DuracionMinutos)
            .GreaterThan(0).When(x => x.DuracionMinutos.HasValue).WithMessage("La duración en minutos debe ser mayor que 0 si se proporciona.");

        
    }
}