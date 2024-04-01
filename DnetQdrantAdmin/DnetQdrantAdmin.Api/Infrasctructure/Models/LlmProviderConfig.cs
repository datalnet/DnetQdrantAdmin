namespace Dnet.QdrantAdmin.Api.Infrasctructure.Models;

public class LlmProviderConfig
{
    public string ApiKey { get; set; } = string.Empty;

    public List<ModelConfig> Models { get; set; } = default!;
}
