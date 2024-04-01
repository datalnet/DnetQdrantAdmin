using Dnet.QdrantAdmin.Application.Shared.Enums;
using Qdrant.Client.Grpc;
using System.ComponentModel.DataAnnotations;

namespace Dnet.QdrantAdmin.Application.Shared.Dtos;

public class QpointDto
{
    public string? QpointId { get; set; }

    [Required]
    public string LlmModel { get; set; } = string.Empty;

    [Required]
    public int Dimension { get; set; }

    public PointId? PointId { get; set; }

    public string CollectionName { get; set; } = string.Empty;

    public float[]? Vectors { get; set; }

    public List<KeyValuePair<string, string>>? Payload { get; set; }

    public string? PayloadString { get; set; }

    public bool HasNum { get; set;} = true;

    public bool HasUuid { get; set; } = false;

    public PointIdType PointIdType { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;
}
