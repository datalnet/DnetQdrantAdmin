using Dnet.QdrantAdmin.Application.Shared.Dtos;
using Qdrant.Client.Grpc;

namespace Dnet.QdrantAdmin.Client.Pages.Admin;

public interface IAdminService
{
    Task<bool> CreateCollection(CreateCollectionDto createCollectionDto);

    Task<List<CollectionDto>> ListCollections();

    Task DeleteCollection(string name);

    Task<CollectionInfoDto> GetCollectionInfo(string text);

    Task<List<QpointDto>> ScrollPoints(ScrollDto scrollDto);

    Task<QpointDto?> RetrievePoint(ScrollDto scrollDto);

    Task CreatePoint(QpointDto pointDto);

    Task<UpdateResult> DeletePoint(DeletePointDto deletePointDto);

    Task<List<QpointDto>> GetImportQPointData(MultipartFormDataContent content);

    Task CreatePoints(CreatePointsDto createPointsDto);
}
