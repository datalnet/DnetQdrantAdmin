using Microsoft.AspNetCore.Mvc;

namespace Dnet.QdrantAdmin.Api.Infrasctructure.Factories;

public interface IProblemDetailFactory
{
    ProblemDetails GetProblemDetail(string type, string details);
}
