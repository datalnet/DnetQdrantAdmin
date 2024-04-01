using Dnet.QdrantAdmin.Application.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Dnet.QdrantAdmin.Api.Infrasctructure.Factories;

public class ProblemDetailFactory : IProblemDetailFactory
{
    public ProblemDetails GetProblemDetail(string problemDetailType, string details)
    {
        if (problemDetailType == ProblemDetailType.DUPLICATE_KEY)
        {
            var response = new ProblemDetails()
            {
                Type = problemDetailType,
                Title = "Duplicate Key",
                Status = 400,
                Detail = details,
            };

            return response;
        }

        if (problemDetailType == ProblemDetailType.INVALID_REQUEST_PAYLOAD)
        {
            var response = new ProblemDetails()
            {
                Type = problemDetailType,
                Title = "Invalid request payload",
                Status = 400,
                Detail = details,
            };

            return response;
        }

        if (problemDetailType == ProblemDetailType.INVALID_OBJECT_ID)
        {
            var response = new ProblemDetails()
            {
                Type = problemDetailType,
                Title = "Invalid request payload",
                Status = 400,
                Detail = details,
            };

            return response;
        }

        if (problemDetailType == ProblemDetailType.INVALID_OBJECT_DATA)
        {
            var response = new ProblemDetails()
            {
                Type = problemDetailType,
                Title = "Invalid request payload",
                Status = 400,
                Detail = details,
            };

            return response;
        }

        if (problemDetailType == ProblemDetailType.RESOURCE_NOT_ALLOWED)
        {
            var response = new ProblemDetails()
            {
                Type = problemDetailType,
                Title = "Resource Not Allowed",
                Status = 400,
                Detail = details,
            };

            return response;
        }

        if (problemDetailType == ProblemDetailType.INVALID_MODEL)
        {
            var response = new ProblemDetails()
            {
                Type = problemDetailType,
                Title = "Invalid Model",
                Status = 400,
                Detail = details,
            };

            return response;
        }

        if (problemDetailType == ProblemDetailType.ACCESS_TOKEN_EXPIRATION)
        {
            var response = new ProblemDetails()
            {
                Type = problemDetailType,
                Title = "Invalid Access Token",
                Status = 401,
                Detail = details,
            };

            return response;
        }

        if (problemDetailType == ProblemDetailType.ACCESS_TOKEN_VALIDATION_FAILURE)
        {
            var response = new ProblemDetails()
            {
                Type = problemDetailType,
                Title = "Invalid Access Token",
                Status = 401,
                Detail = details,
            };

            return response;
        }

        if (problemDetailType == ProblemDetailType.UNAUTHORIZED)
        {
            var response = new ProblemDetails()
            {
                Type = problemDetailType,
                Title = "Invalid Access Token",
                Status = 401,
                Detail = details,
            };

            return response;
        }

        throw new System.Exception("The ProblemDetails for type " + problemDetailType + "is not registered");
    }
}
