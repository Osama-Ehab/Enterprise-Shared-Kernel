using Enterprise.SharedKernel.Enums;
using Enterprise.SharedKernel.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Application.Extensions
{

    public static class ValidatorExtensions
    {

        // Note: No async, no Task, just returning the Result directly
        public static Result ValidateToResult<TDto>(this IValidator<TDto> validator, TDto dto)
        {
            if (dto == null)
                return Result.Failure("Provided data cannot be null.", ErrorType.Validation);


            if (validator == null)
                return Result.Success();

            // Call the synchronous .Validate() instead of .ValidateAsync()
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                string errors = string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure(errors, ErrorType.Validation);
            }

            return Result.Success();
        }
    }
}
