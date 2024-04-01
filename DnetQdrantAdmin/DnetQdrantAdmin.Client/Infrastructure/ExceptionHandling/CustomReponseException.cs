using Dnet.QdrantAdmin.Client.Infrastructure.Models;

namespace Dnet.QdrantAdmin.Client.Infrastructure.ExceptionHandling;

public class CustomReponseException : Exception
{
    public ProblemDetails ProblemDetails { get; set; }


    public CustomReponseException()
    {
    }

    public CustomReponseException(string message, ProblemDetails problemDetails)
        : base(message)
    {
        ProblemDetails = problemDetails;
    }

    public CustomReponseException(string message, Exception inner, ProblemDetails problemDetails)
        : base(message, inner)
    {
        ProblemDetails = problemDetails;
    }
}
