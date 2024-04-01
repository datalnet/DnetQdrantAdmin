namespace Dnet.QdrantAdmin.Application.Shared.Dtos;

public class SearchResultDto
{
    public float Score { get; set; }

    public string? Text { get; set; }

    public string? PayloadString { get; set; }
}
