using System.Net;

namespace TodoApi.Utils.Exceptions;

public class UnprocessableEntityException(string message) : Exception(message)
{
    public HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity;
}
