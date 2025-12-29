using System;
using System.Collections.Generic;

namespace CoreOps.Application.Common.Validation;

public sealed class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Validation failed.")
    {
        var errorList = errors?.ToList() ?? throw new ArgumentNullException(nameof(errors));

        if (errorList.Count == 0)
            throw new ArgumentException("Validation errors collection cannot be empty.", nameof(errors));

        Errors = errorList;
    }
}
