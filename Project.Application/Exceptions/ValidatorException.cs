using System.Net;

namespace Project.Application.Exceptions
{
    public class ValidatorException : BaseCustomException
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
        public override string ErrorCode => "VALIDATION_ERROR";
        public Dictionary<string, string> ValidationErrors { get; } = new Dictionary<string, string>();

        public ValidatorException() { }
        public ValidatorException(string? message) : base(message) { }
        public ValidatorException(string? message, Exception? innerException) : base(message, innerException) { }
        public ValidatorException(string fieldName, string errorMessage) : base("Validation failed")
        {
            ValidationErrors = new Dictionary<string, string>
    {
        { fieldName.ToLower(), errorMessage }
    };
        }
        public ValidatorException(Dictionary<string, string> validationErrors) : base("errors")
        {
            ValidationErrors = validationErrors;
        }
    }
}