namespace Dnet.QdrantAdmin.Api.Infrasctructure.Models;

public class ModelConfig
{
    public string Model { get; set; } = string.Empty;

    public List<int> Distances { get; set; } = new();

    public bool Default { get; set; }
}