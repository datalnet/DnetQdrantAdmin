using Azure.AI.OpenAI;
using Dnet.QdrantAdmin.Api.Infrasctructure.Factories;
using Dnet.QdrantAdmin.Api.Infrasctructure.Models;
using Dnet.QdrantAdmin.Api.Infrasctructure.Services;
using Dnet.QdrantAdmin.Application.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Qdrant.Client.Grpc;

namespace Dnet.QdrantAdmin.Api.Apis;

public static class LlmProviderApi
{
    public static RouteGroupBuilder LlmProviderApis(this RouteGroupBuilder group)
    {
        group.WithTags("LlmProviders");

        group.MapPost("/SimilaritySearch", async ([FromBody] SimilaritySearchDto similaritySearchDto,
                             IOpenAiService openAiService,
                             IQdrantService qdrantService,
                             IProblemDetailFactory problemDetailFactory,
                             HttpContext httpContext) =>
        {

            //if (string.IsNullOrEmpty(similaritySearchDto.Text))
            //{
            //    return Results.BadRequest(problemDetailFactory.GetProblemDetail(ProblemDetailType.INVALID_REQUEST_PAYLOAD.ToString(), "Text can't be empty"));
            //}

            var inputs = new List<string>() { similaritySearchDto.Text };

            var embeddings = await openAiService.GenerateEmbeddingsAsync(inputs, similaritySearchDto.LlmModel, similaritySearchDto.Dimension);

            //if (embeddings.Length != 0)
            //{
            //    return Results.BadRequest("");
            //}

            var scoredPoints = new List<ScoredPoint>();

            var searchResultDtos = new List<SearchResultDto>();

            if (embeddings is not null)
            {
                var item = embeddings.Value.Data[0];

                var embedding = item.Embedding;

                scoredPoints = (await qdrantService.SearchAsync(similaritySearchDto, embedding)).ToList();
            }

            foreach (var scoredPoint in scoredPoints)
            {
                var searchResultDto = new SearchResultDto();

                searchResultDto.Score = scoredPoint.Score;

                searchResultDto.PayloadString = scoredPoint.Payload is not null ? qdrantService.MapFieldToJson(scoredPoint.Payload) : string.Empty;

                if (scoredPoint.Payload != null)
                {
                    foreach (KeyValuePair<string, Value> entry in scoredPoint.Payload)
                    {
                        string key = entry.Key;
                        Value value = entry.Value;

                        switch (key)
                        {
                            case "text":
                                searchResultDto.Text = value.StringValue;
                                break;

                            default:
                                break;
                        }
                    }
                }

                searchResultDtos.Add(searchResultDto);
            }

            return searchResultDtos;
        })
      .WithName("SimilaritySearch")
      .Produces<List<SearchResultDto>>();

        group.MapGet("/GetLlmModels", (
                             IOptions<LlmProviderConfig> config,
                             HttpContext httpContext
                             ) =>
         {
             var llmProvider = new LlmProviderDto();

             var models = new List<ModelDto>();

             foreach (var item in config.Value.Models)
             {
                 var model = new ModelDto()
                 {
                     Model = item.Model,
                     Distances = item.Distances,
                     Default = item.Default
                 };

                 llmProvider.Models.Add(model);
             }

             return llmProvider;
         })
      .WithName("GetLlmModels")
      .Produces<LlmProviderDto>();

        return group;
    }
}
