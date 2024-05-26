using Azure;
using Azure.AI.OpenAI;
using Dnet.QdrantAdmin.Api.Infrasctructure.Models;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;

namespace Dnet.QdrantAdmin.Api.Infrasctructure.Services;

public class OpenAiService : IOpenAiService
{
    private readonly OpenAIClient _openAIClient;

    private readonly IOptions<LlmProviderConfig> _llmProviderConfig;

    public OpenAiService(OpenAIClient openAIClient, IOptions<LlmProviderConfig> llmProviderConfig)
    {
        _openAIClient = openAIClient;

        _llmProviderConfig = llmProviderConfig;
    }

    public async Task<Response<Embeddings>> GenerateEmbeddingsAsync(List<string> inputs, string llmModel, int dimension)
    {
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        var embeddingGenerator = new OpenAITextEmbeddingGenerationService(llmModel, _llmProviderConfig.Value.ApiKey, null, null, null, dimension);
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

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
