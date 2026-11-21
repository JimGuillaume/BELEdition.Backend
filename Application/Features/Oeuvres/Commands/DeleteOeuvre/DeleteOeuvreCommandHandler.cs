using Application.Common.Interfaces;
using Application.Common.Messaging;

namespace Application.Features.Oeuvres.Commands.DeleteOeuvre;

public sealed class DeleteOeuvreCommandHandler(IOeuvreRepository oeuvreRepository)
    : IRequestHandler<DeleteOeuvreCommand, bool>
{
    public async Task<bool> Handle(DeleteOeuvreCommand request, CancellationToken cancellationToken)
    {
        return await oeuvreRepository.DeleteAsync(request.Id, DateTime.UtcNow, cancellationToken);
    }
}
