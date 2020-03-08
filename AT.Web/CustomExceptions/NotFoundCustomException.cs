using System.Net;

namespace AT.CustomExceptions
{
    public class NotFoundCustomException : BaseCustomException
    {
        public NotFoundCustomException(string message, string description, string path) : base(message, description, path, (int)HttpStatusCode.NotFound)
        {
        }
    }
}
