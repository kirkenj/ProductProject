using FluentValidation.Results;

namespace Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public IEnumerable<string> Errors { get; set; }

        public ValidationException(ValidationResult validationResult) 
            : base(string.Join('\n', validationResult.Errors.Select(e => e.ErrorMessage)))
        {
            Errors = validationResult.Errors.Select(e => e.ErrorMessage);
        }
    }
}
