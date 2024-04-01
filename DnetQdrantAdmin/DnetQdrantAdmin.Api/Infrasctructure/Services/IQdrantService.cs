using Dnet.QdrantAdmin.Application.Shared.Dtos;
using Google.Protobuf.Collections;
using Qdrant.Client.Grpc;

namespace Dnet.QdrantAdmin.Api.Infrasctructure.Services;

public interface IQdrantService
{
    Task<bool> CreateCollectionAsync(CreateCollectionDto createCollectionDto);

    Task<List<CollectionDto>> ListCollectionsAsync();

    Task<CollectionInfoDto> GetCollectionInfoAsync(string collectionName);

    Task DeleteCollectionAsync(string collectionName);

    Task<UpdateResult> InsertVectorsAsync(string collectionName, QpointDto pointDto, ReadOnlyMemory<float> vector);

    Task<UpdateResult> InsertVectorsBulkAsync(CreatePointsDto createPointsDto);

    Task<IReadOnlyList<ScoredPoint>> SearchAsync(SimilaritySearchDto similaritySearchDto, ReadOnlyMemory<float> vector);

    Task<QpointDto?> RetrieveAsync(ScrollDto scrollDto);

    Task<List<QpointDto>> ScrollAsync(ScrollDto scrollDto);

    Task<UpdateResult> DeletePointsAsync(DeletePointDto deletePointDto);

    string MapFieldToJson(MapField<string, Value> mapField);
}
