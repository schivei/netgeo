using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetGeo.Json;

public static class GeoExtensions
{
    public static void SetDefaults()
    {
        if (JsonSerializerOptions.Default.Converters.Any(x => x is GeoObjectConverter or CrsConverter))
            return;

        var converters = new List<JsonConverter>(JsonSerializerOptions.Default.Converters)
            {
                new GeoObjectConverter(),
                new CrsConverter()
            };

        Type type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .SingleOrDefault(t => t.FullName == "System.Text.Json.JsonSerializerOptions+ConverterList");
        object[] paramValues = new object[] { JsonSerializerOptions.Default, converters };
        var converterList = type!.GetConstructors()[0].Invoke(paramValues) as IList<JsonConverter>;
        typeof(JsonSerializerOptions).GetRuntimeFields().Single(f => f.Name == "_converters")
        .SetValue(JsonSerializerOptions.Default, converterList);
    }

    private static JsonSerializerOptions Options()
    {
        SetDefaults();

        return JsonSerializerOptions.Default;
    }

    public static string ToGeoJson<T>(this T geoObject) where T : GeoObject =>
        JsonSerializer.Serialize(geoObject, Options());

    public static GeoObject? ToGeoObject(this string geoJson, Type geoType) =>
        JsonSerializer.Deserialize(geoJson, geoType, Options()) as GeoObject;

    public static GeoObject? ToGeoObject(this string geoJson) =>
        JsonSerializer.Deserialize<GeoObject>(geoJson, Options());

    public static T? ToGeoObject<T>(this string geoJson) where T : GeoObject =>
        JsonSerializer.Deserialize<T>(geoJson, Options());
}
