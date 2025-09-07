using Application.DTOs;
using FluentValidation;

namespace Application.Validation;
public class MotoRequestValidator : AbstractValidator<MotoRequest>
{
    public MotoRequestValidator()
    {
        RuleFor(x => x.Modelo).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Placa).NotEmpty().Length(7);
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.Ano).InclusiveBetween(2000, DateTime.UtcNow.Year + 1);
    }
}
