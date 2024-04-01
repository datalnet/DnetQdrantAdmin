using Azure.AI.OpenAI;
using Dnet.QdrantAdmin.Api.Infrasctructure.Services;
using Dnet.QdrantAdmin.Application.Shared.Dtos;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Qdrant.Client.Grpc;
using System.Globalization;

namespace Dnet.QdrantAdmin.Api.Apis;

public static class QdrantApi
{
    public static RouteGroupBuilder QdrantsApis(this RouteGroupBuilder group)
    {
        group.WithTags("Qdrant");

        group.MapPost("/CreateCollection", async ([FromBody] CreateCollectionDto createCollectionDto,
                              IQdrantService qdrantService,
                              HttpContext httpContext
                              ) =>
        {

            var collection = await qdrantService.CreateCollectionAsync(createCollectionDto);

            return collection;
        })
       .WithName("CreateCollection")
       .Produces<bool>();

        group.MapGet("/ListCollections", async (
                             IQdrantService qdrantService,
                             HttpContext httpContext
                             ) =>
        {
            var collection = await qdrantService.ListCollectionsAsync();

            return collection;
        })
      .WithName("ListCollections")
      .Produces<List<CollectionDto>>();

        group.MapPost("/DeleteCollection", async ([FromBody] string name,
                             IQdrantService qdrantService,
                             HttpContext httpContext
                             ) =>
        {
            await qdrantService.DeleteCollectionAsync(name);
        })
      .WithName("DeleteCollection");

        group.MapPost("/GetCollectionInfo", async ([FromBody] string text,
                              IQdrantService qdrantService,
                              HttpContext httpContext
                              ) =>
        {
            var collection = await qdrantService.GetCollectionInfoAsync(text);

            return collection;
        })
       .WithName("GetCollectionInfo")
       .Produces<CollectionInfo>();

        group.MapPost("/ScrollPoints", async ([FromBody] ScrollDto scrollDto,
                            IQdrantService qdrantService,
                            HttpContext httpContext
                            ) =>
        {
            var collection = await qdrantService.ScrollAsync(scrollDto);

            return collection;
        })
     .WithName("ScrollPoints")
     .Produces<List<QpointDto>>();

        group.MapPost("/RetrievePoint", async ([FromBody] ScrollDto scrollDto,
                                              IQdrantService qdrantService,
                                              HttpContext httpContext
                              ) =>
           {
               var collection = await qdrantService.RetrieveAsync(scrollDto);

               return collection;
           })
       .WithName("RetrievePoint")
       .Produces<QpointDto>();

        group.MapPost("/CreatePoint", async ([FromBody] QpointDto pointDto,
                                  IOpenAiService openAiService,
                                  IQdrantService qdrantService,
                                  HttpContext httpContext
                                  ) =>
        {

            var inputs = new List<string>() { pointDto.Text };

            var embeddings = await openAiService.GenerateEmbeddingsAsync(inputs, pointDto.LlmModel, pointDto.Dimension);

            if (embeddings is not null)
            {
                var item = embeddings.Value.Data[0];

                var embedding = item.Embedding;

                var updateResult = await qdrantService.InsertVectorsAsync(pointDto.CollectionName, pointDto, embedding);
            }

        }).WithName("CreatePoint");

        group.MapPost("/CreatePoints", async ([FromBody] CreatePointsDto createPointsDto,
                                       IQdrantService qdrantService,
                                       HttpContext httpContext) =>
        {
            var updateResult = await qdrantService.InsertVectorsBulkAsync(createPointsDto);

        }).WithName("CreatePoints");

        group.MapPost("/DeletePoint", async ([FromBody] DeletePointDto deletePointDto,
                            IQdrantService qdrantService,
                            HttpContext httpContext
                            ) =>
        {
            var result = await qdrantService.DeletePointsAsync(deletePointDto);

            return result;
        })
        .WithName("DeletePoint")
        .Produces<UpdateResult>();

        group.MapPost("/GetImportQPointData", async (HttpRequest request,
                           IQdrantService qdrantService,
                           HttpContext httpContext
                           ) =>
        {
            if (!request.HasFormContentType)
            {
                var tt = request.HasFormContentType;
            }

            long maxFileSize = 1024 * 1024 * 15;

            var _qpoints = new List<QpointDto>();

            var form = await request.ReadFormAsync();
            var file = form.Files["files"];

            if (file is not null)
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = true,
                };

                using (var stream = file.OpenReadStream())
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, config))
                {
                    var items = csv.GetRecords<PointData>();

                    foreach (var item in items)
                    {
                        var pointData = new QpointDto()
                        {
                            Text = item.VectorData,
                            PayloadString = item.Payload,
                        };

                        _qpoints.Add(pointData);
                    }
                }
            }

            return _qpoints;
        })
       .WithName("GetImportQPointData")
       .Produces<List<QpointDto>>();

        return group;
    }
}

public class PointData
{
    public string VectorData { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;
}