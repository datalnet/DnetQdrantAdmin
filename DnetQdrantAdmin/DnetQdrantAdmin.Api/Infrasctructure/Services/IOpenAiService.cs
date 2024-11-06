namespace Dnet.QdrantAdmin.Api.Infrasctructure.Services;

public interface IOpenAiService
{
    Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(List<string> inputs, string llmModel, int dimension);
}
