using Azure;
using Azure.AI.OpenAI;

namespace Dnet.QdrantAdmin.Api.Infrasctructure.Services;

public class OpenAiService : IOpenAiService
{
    private readonly OpenAIClient _openAIClient;

    public OpenAiService(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }

    public async Task<Response<Embeddings>> GenerateEmbeddingsAsync(List<string> inputs, string llmModel, int dimension)
    {
        EmbeddingsOptions embeddingsOptions = new()
        {
            DeploymentName = llmModel,
        };

        if(llmModel != "text-embedding-ada-002")
        {
            embeddingsOptions.Dimensions = dimension;
        }

        foreach (var input in inputs)
        {
            embeddingsOptions.Input.Add(input);
        }

        Response<Embeddings> response = await _openAIClient.GetEmbeddingsAsync(embeddingsOptions);

        return response;
    }
}
