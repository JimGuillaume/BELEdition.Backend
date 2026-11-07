using System.Collections.Concurrent;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Messaging;

// Valide la requête (FluentValidation) puis résout le handler correspondant à son type concret
public sealed class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    private static readonly ConcurrentDictionary<Type, object> Wrappers = new();

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wrapper = (RequestHandlerWrapper<TResponse>)Wrappers.GetOrAdd(
            request.GetType(),
            requestType => Activator.CreateInstance(
                typeof(RequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse)))!);

        return wrapper.Handle(request, serviceProvider, cancellationToken);
    }

    private abstract class RequestHandlerWrapper<TResponse>
    {
        public abstract Task<TResponse> Handle(
            IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    private sealed class RequestHandlerWrapper<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override async Task<TResponse> Handle(
            IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var typedRequest = (TRequest)request;

            await ValidateAsync(typedRequest, serviceProvider, cancellationToken);

            return await serviceProvider
                .GetRequiredService<IRequestHandler<TRequest, TResponse>>()
                .Handle(typedRequest, cancellationToken);
        }

        // Exécute tous les IValidator<TRequest> enregistrés ; lève une ValidationException en cas d'erreurs
        private static async Task ValidateAsync(
            TRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var validators = serviceProvider.GetServices<IValidator<TRequest>>().ToArray();
            if (validators.Length == 0)
            {
                return;
            }

            var context = new ValidationContext<TRequest>(request);
            var failures = new List<FluentValidation.Results.ValidationFailure>();

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken);
                failures.AddRange(result.Errors);
            }

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }
    }
}
