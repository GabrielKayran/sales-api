using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public static class ValidationHelper
{
    public static string BuildErrorMessage(IEnumerable<ValidationFailure> validationErrors)
    { 
        var errors = validationErrors.ToList();
        
        if (!errors.Any())
            return "Erro de validação";

        if (errors.Count == 1)
            return errors.First().ErrorMessage;

        return string.Join("; ", errors.Select(e => e.ErrorMessage));
    }
}
