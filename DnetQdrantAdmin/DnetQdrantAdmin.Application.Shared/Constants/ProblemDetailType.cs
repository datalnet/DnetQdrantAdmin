namespace Dnet.QdrantAdmin.Application.Shared.Constants;

public static class ProblemDetailType
{
    public const string DUPLICATE_KEY = "https://security.datalnet.com/errors/duplicate-key";

    public const string INVALID_REQUEST_PAYLOAD = "https://security.datalnet.com/errors/invalid-request-payload";

    public const string INVALID_OBJECT_ID = "https://security.datalnet.com/errors/invalid-object-id";

    public const string INVALID_OBJECT_DATA = "https://security.datalnet.com/errors/invalid-object-data";

    public const string RESOURCE_NOT_ALLOWED = "https://security.datalnet.com/errors/resource-not-allowed";

    public const string INVALID_MODEL = "https://security.datalnet.com/errors/invalid-model";

    public const string ACCESS_TOKEN_EXPIRATION = "SSX0001";

    public const string ACCESS_TOKEN_VALIDATION_FAILURE = "SSX0002";

    public const string OPERATION_EXCEPTION = "https://security.datalnet.com/errors/operation-exception";

    public const string BAD_REQUEST = "https://security.datalnet.com/errors/bad_request";

    public const string UNAUTHORIZED = "https://security.datalnet.com/errors/unauthorized";

    public const string FORBIDDEN = "https://security.datalnet.com/errors/forbidden";

    public const string REQUESTCANCELLED = "https://security.datalnet.com/errors/request-canceled";
}