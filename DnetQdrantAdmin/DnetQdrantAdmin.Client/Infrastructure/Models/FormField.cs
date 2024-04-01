using System.Text.Json.Serialization;

namespace Dnet.QdrantAdmin.Client.Infrastructure.Models;

public class FormField
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("subfields")]
    public List<FormField>? Subfields { get; set; }

    public int Level { get; set; }

    public int Span { get; set; }
}