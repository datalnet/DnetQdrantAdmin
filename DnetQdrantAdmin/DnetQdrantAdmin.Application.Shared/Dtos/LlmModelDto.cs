using System.ComponentModel.DataAnnotations;

namespace Dnet.QdrantAdmin.Application.Shared.Dtos;

public class ModelDto
{
    [Required]
    public string Model { get; set; } = string.Empty;

    [Required]
    public int Dimension { get; set; } = 1536;

    public List<int> Distances { get; set; } = new();

    public bool Default { get; set; }
}