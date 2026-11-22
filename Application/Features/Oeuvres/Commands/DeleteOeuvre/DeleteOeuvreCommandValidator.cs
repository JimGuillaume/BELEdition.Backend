using FluentValidation;

namespace Application.Features.Oeuvres.Commands.DeleteOeuvre;

public sealed class DeleteOeuvreCommandValidator : AbstractValidator<DeleteOeuvreCommand>
{
    public DeleteOeuvreCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Length(26);
    }
}
