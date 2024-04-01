namespace Dnet.QdrantAdmin.Application.Shared.Dtos;

public class CreatePointsDto
{
    public string LlmModel { get; set; } = string.Empty;

    public int Dimension { get; set; }

    public string CollectionName { get; set; } = string.Empty;

    public List<QpointDto> pointDtos { get; set; } = new();
}
