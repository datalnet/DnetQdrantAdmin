using Qdrant.Client.Grpc;

namespace Dnet.QdrantAdmin.Application.Shared.Dtos;

public class CreateCollectionDto
{
    public string Name { get; set; } = string.Empty;

    public uint ShardNumber { get; set; } = 1;

    public uint ReplicationFactor { get; set; } = 1;

    public uint WriteConsistencyFactor { get; set; } = 1;

    public bool OnDiskPayload { get; set; } = false;

    public string? initFromCollection { get; set; }

    public TimeSpan? Timeout { get; set; }

    public VectorParams VectorParams { get; set; } = new();

    public HnswConfigDiff? HnswConfigDiff { get; set; }

    public OptimizersConfigDiff? OptimizersConfigDiff { get; set; }

    public WalConfigDiff? WalConfigDiff { get; set; }

    public QuantizationConfig? QuantizationConfig { get; set; }

    public ShardingMethod? ShardingMethod { get; set; }

    public SparseVectorConfig? SparseVectorConfig { get; set; }
}