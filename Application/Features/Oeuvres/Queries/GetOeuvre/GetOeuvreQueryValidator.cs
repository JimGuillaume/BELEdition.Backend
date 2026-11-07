using FluentValidation;

namespace Application.Features.Oeuvres.Queries.GetOeuvre;

public sealed class GetOeuvreQueryValidator : AbstractValidator<GetOeuvreQuery>
{

    public GetOeuvreQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .Length(25);
    }
}
