using System;
using System.Collections.Generic;
using CoreOps.Application.Common.Validation;

namespace CoreOps.Application.Operations.CreateRequest;

public static class CreateRequestValidator
{
    public static void Validate(CreateRequestCommand command)
    {
        var errors = new List<string>();

        if (command.TenantId == Guid.Empty)
            errors.Add("TenantId is required.");
        if (string.IsNullOrWhiteSpace(command.Title))
            errors.Add("Title is required.");
        else if (command.Title.Length > 200)
            errors.Add("Title must be 200 characters or fewer.");
        if (command.WorkflowDefinitionId == Guid.Empty)
            errors.Add("WorkflowDefinitionId is required.");
        if (string.IsNullOrWhiteSpace(command.InitialStateCode))
            errors.Add("InitialStateCode is required.");
        if (command.CreatedBy == Guid.Empty)
            errors.Add("CreatedBy is required.");
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
