using System.Reflection;
using System.Text.Json;

namespace NetGeo.Json.SystemText;

public static class GeoExtensions
{
    public static void SetDefaults()
    {
        AppContext.SetSwitch("System.Text.Json.Serialization.Metadata", true);
        AppContext.SetSwitch("System.Text.Json.Serialization.Converters", true);
        AppContext.SetSwitch("System.Text.Json.JsonSerializer.IsReflectionEnabledByDefault", true);

        var jsonOptions = JsonSerializerOptions.Default;

        var field = typeof(JsonSerializerOptions).GetField("_isReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);

        field?.SetValue(jsonOptions, false);

        if (!jsonOptions.Converters.Any(x => x is GeoObjectConverter))
            jsonOptions.Converters.Add(new GeoObjectConverter());

        if (!jsonOptions.Converters.Any(x => x is CrsConverter))
            jsonOptions.Converters.Add(new CrsConverter());

        field.SetValue(jsonOptions, true);
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
