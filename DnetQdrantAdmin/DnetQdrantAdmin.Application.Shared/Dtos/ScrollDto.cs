using Dnet.QdrantAdmin.Application.Shared.Enums;
using Qdrant.Client.Grpc;

namespace Dnet.QdrantAdmin.Application.Shared.Dtos;

public class ScrollDto
{
    public PointId? PointId { get; set; }
    public string? QpointId { get; set; }

    public string CollectionName { get; set; } = string.Empty;

    public PointId? Offset { get; set; }

    public uint Limit { get; set; }

    public bool WithPayload { get; set; }

    public bool WithVector { get; set; }

    public string? OrderByPayloadField { get; set; }

    public bool HasNum { get; set; }

    public bool HasUuid { get; set; }

    public PointIdType PointIdType { get; set; }
}
