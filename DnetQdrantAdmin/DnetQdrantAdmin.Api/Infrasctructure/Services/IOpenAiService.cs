using Azure.AI.OpenAI;
using Azure;

namespace Dnet.QdrantAdmin.Api.Infrasctructure.Services;

public interface IOpenAiService
{
    Task<Response<Embeddings>> GenerateEmbeddingsAsync(List<string> inputs, string llmModel, int dimension);
}
