using FluentValidation;

namespace Application.Features.Oeuvres.Queries.GetOeuvresByUserId;

public sealed class GetOeuvresByUserIdQueryValidator : AbstractValidator<GetOeuvresByUserIdQuery>
{
    public GetOeuvresByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .Length(26);
    }
}
