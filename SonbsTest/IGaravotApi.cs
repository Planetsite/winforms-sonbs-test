using Refit;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SonbsTest;

using ObjectDto = object; // TEMP

public interface IGaravotApi
{
    [Get("/api/v1/contents")]
    Task<ApiResponse<ObjectDto>> GetDelegatesAsync();
}

public static class GaravotApiFactory
{
    public static IGaravotApi Create(HttpClient http)
        => RestService.For<IGaravotApi>(http, _refitSettings);

    private static readonly RefitSettings _refitSettings = new RefitSettings
    {
        ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { (JsonConverter)new JsonStringEnumConverter() }
        })
    };
}
