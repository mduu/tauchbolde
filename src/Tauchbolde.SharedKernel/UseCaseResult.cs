using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using JetBrains.Annotations;

namespace Tauchbolde.SharedKernel
{
    public class UseCaseResult
    {
        public UseCaseResult(
            [CanBeNull] IEnumerable<ValidationFailure> errors = null,
            ResultCategory? resultCategory = null)
        {
            Errors = errors ?? Enumerable.Empty<ValidationFailure>();

            ResultCategory = resultCategory ??
                             (Errors.Any()
                                 ? ResultCategory.GeneralFailure
                                 : ResultCategory.Success);
        }

        [NotNull]
        public static UseCaseResult Success() => new UseCaseResult();

        [NotNull]
        public static UseCaseResult Fail(
            IEnumerable<ValidationFailure> errors = null,
            ResultCategory resultCategory = ResultCategory.GeneralFailure)
            => new UseCaseResult(errors, resultCategory);

        [NotNull]
        public static UseCaseResult NotFound(IEnumerable<ValidationFailure> errors = null)
            => new UseCaseResult(errors, ResultCategory.NotFound);

        [NotNull]
        public static UseCaseResult ValidationFailed(IEnumerable<ValidationFailure> errors = null)
            => new UseCaseResult(errors, ResultCategory.ValidationFailed);

        public bool IsSuccessful => ResultCategory == ResultCategory.Success;

        public ResultCategory ResultCategory { get; }

        [NotNull] public IEnumerable<ValidationFailure> Errors { get; }
    }

    public class UseCaseResult<TPayload> : UseCaseResult
    {
        public UseCaseResult(
            [CanBeNull] IEnumerable<ValidationFailure> errors = null,
            [CanBeNull] TPayload payload = default,
            [CanBeNull] ResultCategory? resultCategory = null)
            : base(errors, resultCategory)
        {
            Payload = payload;
        }

        [CanBeNull] public TPayload Payload { get; }

        [NotNull]
        public static UseCaseResult<TPayload> Success(TPayload payload) =>
            new UseCaseResult<TPayload>(payload: payload);

        [NotNull]
        public new static UseCaseResult<TPayload> Fail(IEnumerable<ValidationFailure> errors = null,
            ResultCategory? resultCategory = null)
            => new UseCaseResult<TPayload>(errors, resultCategory: resultCategory);

        [NotNull]
        public new static UseCaseResult<TPayload> NotFound(IEnumerable<ValidationFailure> errors = null)
            => new UseCaseResult<TPayload>(errors, resultCategory: ResultCategory.NotFound);

        [NotNull]
        public static UseCaseResult<TPayload> NotFound(string objectName, string propertyName, Guid id)
            => new UseCaseResult<TPayload>(
                new List<ValidationFailure>
                {
                    new ValidationFailure(
                        propertyName,
                        $"{objectName} with ID [{id}] ({propertyName}) not found!", id)
                },
                resultCategory: ResultCategory.NotFound);

        [NotNull]
        public new static UseCaseResult<TPayload> ValidationFailed(IEnumerable<ValidationFailure> errors = null)
            => new UseCaseResult<TPayload>(errors, resultCategory: ResultCategory.ValidationFailed);
    }
}