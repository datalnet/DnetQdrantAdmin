using Qdrant.Client.Grpc;

namespace Dnet.QdrantAdmin.Application.Shared.Dtos;

public class CollectionInfoDto
{
    public string? Status { get; set; }

    public ulong VectorsCount { get; set; }

    public ulong SegmentsCount { get; set; }

    public ulong PointsCount { get; set; }

    public ulong IndexedVectorsCount { get; set; }

    public ulong M { get; set; }

    public ulong EfConstruct { get; set; }

    public ulong FullScanThreshold { get; set; }

    public ulong MaxIndexingThreads { get; set;}

    public bool OnDisk { get; set;}

    public ulong IndexingThreshold { get; set;}

    public bool OnDiskPayload { get; set; }

    public ulong Dimension { get; set; }

    public string Distance { get; set; } = string.Empty;

    public ulong WalCapacityMb { get; set; }
}
