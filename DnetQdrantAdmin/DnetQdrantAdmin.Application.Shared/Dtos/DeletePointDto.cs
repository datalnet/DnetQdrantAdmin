using Qdrant.Client.Grpc;

namespace Dnet.QdrantAdmin.Application.Shared.Dtos;

public class DeletePointDto
{
    public string CollectionName { get; set; } = string.Empty;

    public List<string> Ids { get; set; } = [];

    public bool Wait { get; set; } = true;

    public WriteOrderingType? WriteOrderingType { get; set; } = null;

    public ShardKeySelector? ShardKeySelector { get; set; } = null;
}
