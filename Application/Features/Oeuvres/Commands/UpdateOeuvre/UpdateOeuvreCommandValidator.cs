using FluentValidation;

namespace Application.Features.Oeuvres.Commands.UpdateOeuvre;

public sealed class UpdateOeuvreCommandValidator : AbstractValidator<UpdateOeuvreCommand>
{
    public UpdateOeuvreCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Length(26);

        RuleFor(x => x.Titre)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.CouverturePremiere)
            .MaximumLength(500);

        RuleFor(x => x.CouvertureQuatrieme)
            .MaximumLength(500);
    }
}
