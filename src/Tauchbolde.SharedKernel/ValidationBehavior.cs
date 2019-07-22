using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Tauchbolde.SharedKernel
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);
            var failures = validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                if (typeof(TResponse).IsAssignableFrom(typeof(UseCaseResult)))
                {
                    return (TResponse) Activator.CreateInstance(typeof(TResponse), failures);
                }
                
                throw new ValidationException(failures);
            }

            return await next();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);
            var failures = validators
                .Select(async v => (await v.ValidateAsync(context, cancellationToken)))
                .SelectMany(result => result.Result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                if (typeof(TResponse).IsAssignableFrom(typeof(UseCaseResult)))
                {
                    return (TResponse) Activator.CreateInstance(typeof(TResponse), failures);
                }

                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}