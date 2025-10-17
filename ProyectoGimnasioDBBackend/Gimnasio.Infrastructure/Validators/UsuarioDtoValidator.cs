using FluentValidation;
using Gimnasio.Core.DTOs;

namespace Gimnasio.Infrastructure.Validators;
public class UsuarioDtoValidator : AbstractValidator<UsuarioDto>
{
    public UsuarioDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres.");

        RuleFor(x => x.Edad)
            .InclusiveBetween(10, 100).WithMessage("La edad debe estar entre 10 y 100 años.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio.");

        RuleFor(x => x.IsActive)
            .Must(value => value == 0 || value == 1).WithMessage("IsActive debe ser 0 o 1.");
        
        
    }
}