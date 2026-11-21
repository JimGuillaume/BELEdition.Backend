using FluentValidation;

namespace Application.Features.Oeuvres.Commands.CreateOeuvre;

public sealed class CreateOeuvreCommandValidator : AbstractValidator<CreateOeuvreCommand>
{
    public CreateOeuvreCommandValidator()
    {
        RuleFor(x => x.Titre)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.CouverturePremiere)
            .MaximumLength(500);

        RuleFor(x => x.CouvertureQuatrieme)
            .MaximumLength(500);

        RuleFor(x => x.IdAuteur)
            .NotEmpty()
            .Length(26);

        RuleFor(x => x.IdType)
            .NotEmpty()
            .Length(26);

        RuleFor(x => x.IdStatus)
            .NotEmpty()
            .Length(26);
    }
}
