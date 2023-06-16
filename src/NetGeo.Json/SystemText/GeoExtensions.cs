using System.Text.Json;

namespace NetGeo.Json.SystemText;

public static class GeoExtensions
{
    public static void SetDefaults()
    {
    }

    private static JsonSerializerOptions Options()
    {
        var opts = new JsonSerializerOptions();
        opts.Converters.Add(new GeoObjectConverter());

        return opts;
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
