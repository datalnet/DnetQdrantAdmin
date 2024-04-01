using Dnet.QdrantAdmin.Application.Shared.Dtos;

namespace Dnet.QdrantAdmin.Client.Pages.TherapyNotes;

public interface ILlmProviderService
{
    Task<ReadOnlyMemory<float>> GenerateEmbeddings(string text);

    Task<List<SearchResultDto>> SimilaritySearch(SimilaritySearchDto similaritySearchDto);

    Task<LlmProviderDto> GetLlmModels();
}
