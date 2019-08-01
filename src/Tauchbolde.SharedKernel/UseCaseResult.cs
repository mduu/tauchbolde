using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using JetBrains.Annotations;

namespace Tauchbolde.SharedKernel
{
    public class UseCaseResult
    {
        public UseCaseResult()
        {
        }

        public UseCaseResult([CanBeNull] IEnumerable<ValidationFailure> errors = null)
        {
            Errors = errors ?? Enumerable.Empty<ValidationFailure>();
        }
      
        [NotNull] public static UseCaseResult Success() => new UseCaseResult();
        
        [NotNull] public static UseCaseResult Fail(IEnumerable<ValidationFailure> errors = null) => new UseCaseResult(errors);
        
        public bool IsSuccessful => !Errors.Any();
        
        [NotNull] public IEnumerable<ValidationFailure> Errors { get; } = Enumerable.Empty<ValidationFailure>();
    }

    public class UseCaseResult<TPayload> : UseCaseResult
    {
        public UseCaseResult(
            [CanBeNull] IEnumerable<ValidationFailure> errors = null,
            [CanBeNull] TPayload payload = default)
            : base(errors)
        {
            Payload = payload;
        }

        [CanBeNull] public TPayload Payload { get; }
        
        [NotNull] public static UseCaseResult<TPayload> Success(TPayload payload) => new UseCaseResult<TPayload>(payload: payload);
        
        [NotNull] public new static UseCaseResult<TPayload> Fail(IEnumerable<ValidationFailure> errors = null) => new UseCaseResult<TPayload>(errors);
    }
}