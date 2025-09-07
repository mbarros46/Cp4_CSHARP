using Application.DTOs;
using FluentValidation;

namespace Application.Validation;
public class PatioRequestValidator : AbstractValidator<PatioRequest>
{
    public PatioRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do pátio é obrigatório.");
        RuleFor(x => x.Endereco)
            .NotEmpty().WithMessage("O endereço do pátio é obrigatório.");
        RuleFor(x => x.Capacidade)
            .GreaterThan(0).WithMessage("A capacidade deve ser maior que zero.");
    }
}
