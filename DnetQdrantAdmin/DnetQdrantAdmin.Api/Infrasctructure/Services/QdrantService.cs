using Qdrant.Client.Grpc;
using Qdrant.Client;
using Dnet.QdrantAdmin.Application.Shared.Dtos;
using Google.Protobuf.Collections;
using Dnet.QdrantAdmin.Application.Shared.Enums;
using Value = Qdrant.Client.Grpc.Value;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using Dnet.QdrantAdmin.Api.Infrasctructure.Models;
using Google.Protobuf;

namespace Dnet.QdrantAdmin.Api.Infrasctructure.Services;

public class QdrantService : IQdrantService
{
    private readonly QdrantClient _client;
    private readonly IOpenAiService _openAiService;

    public QdrantService(IOptions<QdrantConfig> config, IOpenAiService openAiService)
    {
        _client = new QdrantClient(config.Value.QdrantServerHost);

        _openAiService = openAiService;
    }

    public async Task<bool> CreateCollectionAsync(CreateCollectionDto createCollectionDto)
    {
        createCollectionDto.VectorParams.Size = createCollectionDto.VectorParams.Size > 0 ? createCollectionDto.VectorParams.Size : 100;
        createCollectionDto.OptimizersConfigDiff.VacuumMinVectorNumber = createCollectionDto.OptimizersConfigDiff.VacuumMinVectorNumber > 0 ?
                                                                       createCollectionDto.OptimizersConfigDiff.VacuumMinVectorNumber : 100;

        await _client.CreateCollectionAsync(
               createCollectionDto.Name,
               createCollectionDto.VectorParams,
               createCollectionDto.ShardNumber,
               createCollectionDto.ReplicationFactor,
               createCollectionDto.WriteConsistencyFactor,
               createCollectionDto.OnDiskPayload,
               createCollectionDto.HnswConfigDiff,
               createCollectionDto.OptimizersConfigDiff,
               createCollectionDto.WalConfigDiff,
               createCollectionDto.QuantizationConfig,
               createCollectionDto.initFromCollection,
               createCollectionDto.ShardingMethod,
               createCollectionDto.SparseVectorConfig,
               createCollectionDto.Timeout,
               CancellationToken.None
               );

        return true;
    }

    public async Task<CollectionInfoDto> GetCollectionInfoAsync(string collectionName)
    {
        var result = await _client.GetCollectionInfoAsync(collectionName);

        var collectionInfo = new CollectionInfoDto()
        {
            Status = result.Status.ToString(),
            VectorsCount = result.VectorsCount,
            SegmentsCount = result.SegmentsCount,
            PointsCount = result.PointsCount,
            IndexedVectorsCount = result.IndexedVectorsCount,
            M = result.Config.HnswConfig.M,
            EfConstruct = result.Config.HnswConfig.EfConstruct,
            FullScanThreshold = result.Config.HnswConfig.FullScanThreshold,
            MaxIndexingThreads = result.Config.HnswConfig.MaxIndexingThreads,
            OnDisk = result.Config.HnswConfig.OnDisk,
            IndexingThreshold = result.Config.OptimizerConfig.IndexingThreshold,
            OnDiskPayload = result.Config.Params.OnDiskPayload,
            Dimension = result.Config.Params.VectorsConfig.Params.Size,
            Distance = result.Config.Params.VectorsConfig.Params.Distance.ToString(),
            WalCapacityMb = result.Config.WalConfig.WalCapacityMb,
        };

        return collectionInfo;
    }

    public async Task DeleteCollectionAsync(string collectionName)
    {
        await _client.DeleteCollectionAsync(collectionName);
    }

    public async Task<List<CollectionDto>> ListCollectionsAsync()
    {
        var result = await _client.ListCollectionsAsync();

        var collections = new List<CollectionDto>();

        foreach (var item in result)
        {
            var collection = new CollectionDto()
            {
                Name = item
            };

            collections.Add(collection);
        }

        return collections;
    }

    public async Task<UpdateResult> InsertVectorsAsync(string collectionName, QpointDto pointDto, ReadOnlyMemory<float> vector)
    {
        var point = new PointStruct();

        if (pointDto.HasUuid)
        {
            Guid newGuid = Guid.NewGuid();

            point.Id = newGuid;
            point.Vectors = vector.ToArray();
        }
        else
        {
            var count = await _client.CountAsync(collectionName);

            point.Id = count + 1;
            point.Vectors = vector.ToArray();
        }

        if (!string.IsNullOrEmpty(pointDto.PayloadString)) JsonToMapField(pointDto.PayloadString, point.Payload);

        var points = new List<PointStruct>
        {
            point
        };

        return await _client.UpsertAsync(collectionName, points);
    }

    public async Task<UpdateResult> InsertVectorsBulkAsync(CreatePointsDto createPointsDto)
    {
        var points = new List<PointStruct>();

        var count = await _client.CountAsync(createPointsDto.CollectionName);

        var inputs = createPointsDto.pointDtos.Select(p => p.Text).ToList();

        var embeddings = await _openAiService.GenerateEmbeddingsAsync(inputs, createPointsDto.LlmModel, createPointsDto.Dimension);

        for (int i = 0; i < createPointsDto.pointDtos.Count; i++)
        {
            var pointDto = createPointsDto.pointDtos[i];

            var embedding = embeddings[0];

            var point = new PointStruct();

            if (pointDto.HasUuid)
            {
                Guid newGuid = Guid.NewGuid();

                point.Id = newGuid;
                point.Vectors = embedding.ToArray();
            }
            else
            {
                point.Id = count++;
                point.Vectors = embedding.ToArray();
            }

            if (!string.IsNullOrEmpty(pointDto.PayloadString)) JsonToMapField(pointDto.PayloadString, point.Payload);

            points.Add(point);
        }

        return await _client.UpsertAsync(createPointsDto.CollectionName, points);
    }

    public MapField<string, Value> JsonToMapField(string json, MapField<string, Value> result)
    {
        using JsonDocument doc = JsonDocument.Parse(json);

        foreach (JsonProperty property in doc.RootElement.EnumerateObject())
        {
            result[property.Name] = ConvertJsonElementToValue(property.Value);
        }

        return result;
    }

    public Value JsonToValue(string json)
    {
        using JsonDocument doc = JsonDocument.Parse(json);

        return ConvertJsonElementToValue(doc.RootElement);
    }

    private Value ConvertJsonElementToValue(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var structValue = new Struct();
                foreach (JsonProperty prop in element.EnumerateObject())
                {
                    structValue.Fields[prop.Name] = ConvertJsonElementToValue(prop.Value);
                }
                return new Value { StructValue = structValue };

            case JsonValueKind.Array:
                var listValue = new ListValue();
                foreach (JsonElement item in element.EnumerateArray())
                {
                    listValue.Values.Add(ConvertJsonElementToValue(item));
                }
                return new Value { ListValue = listValue };

            case JsonValueKind.Number:
                if (element.TryGetInt64(out long intValue))
                {
                    return new Value { IntegerValue = intValue };
                }
                else
                {
                    return new Value { DoubleValue = element.GetDouble() };
                }

            case JsonValueKind.String:
                return new Value { StringValue = element.GetString() };

            case JsonValueKind.True:
            case JsonValueKind.False:
                return new Value { BoolValue = element.GetBoolean() };

            case JsonValueKind.Null:
                return new Value { NullValue = NullValue.NullValue };

            default:
                throw new ArgumentException($"Unsupported JSON value kind: {element.ValueKind}");
        }
    }

    public async Task<List<QpointDto>> ScrollAsync(ScrollDto scrollDto)
    {
        var scrollResponse = await _client.ScrollAsync(scrollDto.CollectionName, null, scrollDto.Limit, scrollDto.Offset);

        var collections = new List<QpointDto>();

        foreach (var item in scrollResponse.Result)
        {
            var collection = new QpointDto()
            {
                CollectionName = scrollDto.CollectionName,
                QpointId = item.Id.HasNum ? item.Id.Num.ToString() : item.Id.HasUuid ? item.Id.Uuid : string.Empty,
                PayloadString = MapFieldToJson(item.Payload),
                HasNum = item.Id.HasNum,
                HasUuid = item.Id.HasUuid,
                PointIdType = item.Id.HasNum ? PointIdType.Numerical : item.Id.HasUuid ? PointIdType.Uuid : PointIdType.None,
                PointId = item.Id,
            };

            collections.Add(collection);
        }

        return collections;
    }

    public async Task<QpointDto?> RetrieveAsync(ScrollDto scrollDto)
    {
        var scrollResponse = new List<RetrievedPoint>();

        switch (scrollDto.PointIdType)
        {
            case PointIdType.None:
                break;

            case PointIdType.Numerical:

                bool result = ulong.TryParse(scrollDto.QpointId, out ulong value);

                if (result)
                {
                    scrollResponse = (await _client.RetrieveAsync(scrollDto.CollectionName, value, scrollDto.WithPayload, scrollDto.WithVector)).ToList();
                }

                break;

            case PointIdType.Uuid:

                bool result1 = Guid.TryParse(scrollDto.QpointId, out Guid guid);

                if (result1)
                {
                    scrollResponse = (await _client.RetrieveAsync(scrollDto.CollectionName, guid, scrollDto.WithPayload, scrollDto.WithVector)).ToList();
                }

                break;
        }

        var collections = new List<QpointDto>();

        foreach (var item in scrollResponse)
        {
            var collection = new QpointDto()
            {
                CollectionName = scrollDto.CollectionName,
                QpointId = item.Id.HasNum ? item.Id.Num.ToString() : item.Id.HasUuid ? item.Id.Uuid : string.Empty,
                Vectors = scrollDto.WithVector ? item.Vectors.Vector.Data.ToArray() : null,
                PayloadString = MapFieldToJson(item.Payload),
            };

            collections.Add(collection);
        }

        return collections.Any() ? collections.FirstOrDefault() : new QpointDto();
    }

    public async Task<IReadOnlyList<ScoredPoint>> SearchAsync(SimilaritySearchDto similaritySearchDto, ReadOnlyMemory<float> vector)
    {
        var filter = !string.IsNullOrEmpty(similaritySearchDto.FilterString) ? Filter.Parser.ParseJson(similaritySearchDto.FilterString) : null;

        var points = await _client.SearchAsync(similaritySearchDto.CollectionName, vector, filter: filter, searchParams: similaritySearchDto.SearchParams,
                                               limit: similaritySearchDto.Limit, offset: similaritySearchDto.Offset, payloadSelector: similaritySearchDto.WithPayloadSelector,
                                               vectorsSelector: similaritySearchDto.WithVectorsSelector, scoreThreshold: similaritySearchDto.ScoreThreshold,
                                               vectorName: similaritySearchDto.VectorName, timeout: similaritySearchDto.Timeout);

        return points;
    }

    public async Task<UpdateResult> DeletePointsAsync(DeletePointDto deletePointDto)
    {
        List<ulong> ulongIds = [];
        List<Guid> guidIds = [];

        foreach (var id in deletePointDto.Ids)
        {
            if (ulong.TryParse(id, out ulong longId))
            {
                ulongIds.Add(longId);
            }
            else if (Guid.TryParse(id, out Guid guidId))
            {
                guidIds.Add(guidId);
            }
            else
            {
                throw new ArgumentException($"Formato de ID no válido: {id}");
            }
        }

        UpdateResult updateResult = new();

        if (ulongIds.Any())
        {
            updateResult = await _client.DeleteAsync(deletePointDto.CollectionName, ulongIds, deletePointDto.Wait, deletePointDto.WriteOrderingType, deletePointDto.ShardKeySelector);

            IReadOnlyList<string> names = new List<string>();

            await _client.DeleteVectorsAsync(deletePointDto.CollectionName, names, ulongIds, deletePointDto.Wait);
        }

        if (guidIds.Any())
        {
            updateResult = await _client.DeleteAsync(deletePointDto.CollectionName, guidIds, deletePointDto.Wait, deletePointDto.WriteOrderingType, deletePointDto.ShardKeySelector);

            IReadOnlyList<string> names = new List<string>();

            await _client.DeleteVectorsAsync(deletePointDto.CollectionName, names, guidIds, deletePointDto.Wait);
        }

        return updateResult;
    }

    public string MapFieldToJson(MapField<string, Value> mapField)
    {
        using var stream = new MemoryStream();

        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject(); // Start of JSON object

            foreach (var entry in mapField)
            {
                writer.WritePropertyName(entry.Key);
                WriteValue(writer, entry.Value); // Write each Value object to JSON
            }

            writer.WriteEndObject(); // End of JSON object
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static void WriteValue(Utf8JsonWriter writer, Value value)
    {
        switch (value.KindCase)
        {
            case Value.KindOneofCase.StringValue:
                writer.WriteStringValue(value.StringValue);
                break;
            case Value.KindOneofCase.IntegerValue:
                writer.WriteNumberValue(value.IntegerValue);
                break;
            case Value.KindOneofCase.DoubleValue:
                writer.WriteNumberValue(value.DoubleValue);
                break;
            case Value.KindOneofCase.BoolValue:
                writer.WriteBooleanValue(value.BoolValue);
                break;
            case Value.KindOneofCase.ListValue:
                writer.WriteStartArray(); // Start of a list
                foreach (var item in value.ListValue.Values)
                {
                    WriteValue(writer, item); // Recursively write the list items
                }
                writer.WriteEndArray(); // End of a list
                break;
            case Value.KindOneofCase.StructValue:
                writer.WriteStartObject(); // Start of an object
                foreach (var field in value.StructValue.Fields)
                {
                    writer.WritePropertyName(field.Key);
                    WriteValue(writer, field.Value); // Recursively write the object properties
                }
                writer.WriteEndObject(); // End of an object
                break;
            case Value.KindOneofCase.NullValue:
                writer.WriteNullValue();
                break;
            // Add other cases as needed
            default:
                throw new ArgumentException($"Unsupported Value kind: {value.KindCase}");
        }
    }
}

//var channel = QdrantChannel.ForAddress($"https://{host}:6334");

//var grpcClient = new QdrantGrpcClient(channel);

//_client = new QdrantClient(grpcClient);


//foreach (var pointDto in pointDtos)
//{
//    var point = new PointStruct();

//    if (pointDto.HasUuid)
//    {
//        Guid newGuid = Guid.NewGuid();

//        point.Id = newGuid;
//        point.Vectors = embeddings.ToArray();
//    }
//    else
//    {
//        point.Id = count++;
//        point.Vectors = embeddings.ToArray();
//    }

//    if (!string.IsNullOrEmpty(pointDto.PayloadString)) JsonToMapField(pointDto.PayloadString, point.Payload);

//    points.Add(point);
//}