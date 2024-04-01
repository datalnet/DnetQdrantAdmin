using Dnet.QdrantAdmin.Application.Shared.Dtos;
using Qdrant.Client.Grpc;
using System.Net.Http.Json;

namespace Dnet.QdrantAdmin.Client.Pages.Admin;

public class AdminService : IAdminService
{
    private readonly HttpClient _httpClient;

    public AdminService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CreateCollection(CreateCollectionDto createCollectionDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/Qdrants/CreateCollection", createCollectionDto);

        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<List<CollectionDto>> ListCollections()
    {
        return await _httpClient.GetFromJsonAsync<List<CollectionDto>>($"api/Qdrants/ListCollections");
    }

    public async Task DeleteCollection(string name)
    {
        await _httpClient.PostAsJsonAsync($"api/Qdrants/DeleteCollection", name);
    }

    public async Task<CollectionInfoDto> GetCollectionInfo(string text)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/Qdrants/GetCollectionInfo", text);

        return await response.Content.ReadFromJsonAsync<CollectionInfoDto>();
    }

    public async Task<List<QpointDto>> ScrollPoints(ScrollDto scrollDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/Qdrants/ScrollPoints", scrollDto);

        return await response.Content.ReadFromJsonAsync<List<QpointDto>>();
    }

    public async Task<QpointDto?> RetrievePoint(ScrollDto scrollDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/Qdrants/RetrievePoint", scrollDto);

        return await response.Content.ReadFromJsonAsync<QpointDto?>();
    }

    public async Task CreatePoint(QpointDto pointDto)
    {
        await _httpClient.PostAsJsonAsync($"api/Qdrants/CreatePoint", pointDto);
    }

    public async Task<UpdateResult> DeletePoint(DeletePointDto deletePointDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/Qdrants/DeletePoint", deletePointDto);

        return await response.Content.ReadFromJsonAsync<UpdateResult>();
    }

    public async Task<List<QpointDto>> GetImportQPointData(MultipartFormDataContent content)
    {
        var response = await _httpClient.PostAsync($"api/Qdrants/GetImportQPointData", content);

        return await response.Content.ReadFromJsonAsync<List<QpointDto>>();
    }

    public async Task CreatePoints(CreatePointsDto createPointsDto)
    {
        var url = $"api/Qdrants/CreatePoints";

        await _httpClient.PostAsJsonAsync(url, createPointsDto);
    }
}
