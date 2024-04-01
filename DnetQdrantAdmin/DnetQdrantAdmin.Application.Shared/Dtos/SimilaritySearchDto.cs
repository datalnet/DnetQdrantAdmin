using Qdrant.Client.Grpc;
using System.ComponentModel.DataAnnotations;

namespace Dnet.QdrantAdmin.Application.Shared.Dtos;

public class SimilaritySearchDto
{
    [Required]
    public string CollectionName { get; set; } = string.Empty;

    [Required]
    public string LlmModel { get; set; } = string.Empty;

    [Required]
    public int Dimension { get; set; }

    [Required]
    public string? Text { get; set; }

    [Required]
    public ulong Limit { get; set; } = 5;

    public Filter? Filter { get; set; }

    public string? FilterString { get; set; } = string.Empty;

    public SearchParams? SearchParams { get; set; }

    public ulong Offset { get; set; } = 0;

    public WithPayloadSelector? WithPayloadSelector { get; set; }

    public WithVectorsSelector? WithVectorsSelector { get; set; }

    public float? ScoreThreshold { get; set; }

    public string? VectorName { get; set; }

    public ReadConsistency? ReadConsistency { get; set; }

    public ShardKeySelector? ShardKeySelector { get; set; }

    public TimeSpan? Timeout { get; set; }
}
