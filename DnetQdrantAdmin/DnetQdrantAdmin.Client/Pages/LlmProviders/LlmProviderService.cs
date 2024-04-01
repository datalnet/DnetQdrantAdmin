using Dnet.QdrantAdmin.Application.Shared.Dtos;
using System.Net.Http.Json;

namespace Dnet.QdrantAdmin.Client.Pages.TherapyNotes;

public class LlmProviderService : ILlmProviderService
{
    private readonly HttpClient _httpClient;

    public LlmProviderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ReadOnlyMemory<float>> GenerateEmbeddings(string text)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/LlmProviders/GenerateEmbeddings", text);

        return await response.Content.ReadFromJsonAsync<ReadOnlyMemory<float>>();
    }

    public async Task<List<SearchResultDto>> SimilaritySearch(SimilaritySearchDto similaritySearchDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/LlmProviders/SimilaritySearch", similaritySearchDto);

        return await response.Content.ReadFromJsonAsync<List<SearchResultDto>>();
    }

    public async Task<LlmProviderDto> GetLlmModels()
    {
        return await _httpClient.GetFromJsonAsync<LlmProviderDto>($"api/LlmProviders/GetLlmModels");
    }
}
