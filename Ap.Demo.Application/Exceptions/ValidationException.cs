

namespace Ap.Demo.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base()
        {

        }
        public ValidationException(string message) : base(message) { }
    }
}
