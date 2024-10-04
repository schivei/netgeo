using System.Text.Json;

namespace NetGeo.Json.SystemText;

public static class GeoExtensions
{
    public static void SetDefaults()
    {
        if (JsonSerializerOptions.Default.Converters.Any(x => x is GeoObjectConverter or CrsConverter))
            return;

        JsonSerializerOptions.Default
            .Converters.Add(new GeoObjectConverter());

        JsonSerializerOptions.Default
            .Converters.Add(new CrsConverter());
    }

    private static JsonSerializerOptions Options()
    {
        SetDefaults();

        return JsonSerializerOptions.Default;
    }

    public static string ToGeoJson<T>(this T geoObject) =>
        JsonSerializer.Serialize(geoObject, Options());

    public static GeoObject? ToGeoObject(this string geoJson, Type geoType) =>
        JsonSerializer.Deserialize(geoJson, geoType, Options()) as GeoObject;

    public static GeoObject? ToGeoObject(this string geoJson) =>
        JsonSerializer.Deserialize<GeoObject>(geoJson, Options());

    public static T? ToGeoObject<T>(this string geoJson) =>
        JsonSerializer.Deserialize<T>(geoJson, Options());
}
