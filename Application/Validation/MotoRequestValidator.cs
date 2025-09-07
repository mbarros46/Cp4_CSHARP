using Application.DTOs;
using FluentValidation;

namespace Application.Validation;
public class MotoRequestValidator : AbstractValidator<MotoRequest>
{
    public MotoRequestValidator()
    {
        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("O modelo é obrigatório.")
            .MaximumLength(100).WithMessage("O modelo deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Placa)
            .NotEmpty().WithMessage("A placa é obrigatória.")
            .Length(7).WithMessage("A placa deve ter 7 caracteres.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("O status é obrigatório.")
            .Must(BeAValidStatus)
            .WithMessage("Status deve ser um dos seguintes valores: Active, Inactive, Desactive.");

        RuleFor(x => x.Ano)
            .InclusiveBetween(2000, DateTime.UtcNow.Year + 1)
            .WithMessage($"Ano deve estar entre 2000 e {DateTime.UtcNow.Year + 1}.");
    }

    private bool BeAValidStatus(string status)
    {
        return status == "Active" || status == "Inactive" || status == "Desactive";
    }
}
