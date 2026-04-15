using Application.Helpers;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Application;
public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (!failures.Any())
            return await next();

        // Hata mesajlarını birleştir
        var errorMessage = string.Join(" | ", failures.Select(f => f.ErrorMessage));

        // TResponse → Result<T> ise reflection ile Fail döndür
        var responseType = typeof(TResponse);

        if (responseType.IsGenericType &&
            responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var innerType = responseType.GetGenericArguments()[0];

            var failMethod = typeof(Result<>)
                .MakeGenericType(innerType)
                .GetMethod("Fail", BindingFlags.Public | BindingFlags.Static);

            var result = failMethod!.Invoke(null, [errorMessage]);

            return (TResponse)result!;
        }

        // Result<T> değilse eski davranış — throw
        throw new ValidationException(failures);
    }
}

