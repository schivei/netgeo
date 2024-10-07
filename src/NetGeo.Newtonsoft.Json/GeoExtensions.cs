using System.Globalization;
using Newtonsoft.Json;

namespace NetGeo.Json;

public static class GeoExtensions
{
    public static void SetDefaults()
    {
        var settings = JsonConvert.DefaultSettings?.Invoke() ?? new JsonSerializerSettings();

        settings.Converters = new HashSet<JsonConverter>(settings.Converters)
        {
            new CrsConverter(),
            new GeoObjectConverter(),
        }.ToList();

        settings.Culture = CultureInfo.InvariantCulture;
        settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        settings.TypeNameHandling = TypeNameHandling.Auto;
        settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
        settings.NullValueHandling = NullValueHandling.Ignore;

        JsonConvert.DefaultSettings = () => settings;
    }

    private static JsonSerializerSettings Options()
    {
        SetDefaults();

        return JsonConvert.DefaultSettings.Invoke();
    }

    public static string ToGeoJson<T>(this T geoObject) =>
        JsonConvert.SerializeObject(geoObject, Options());

    public static GeoObject? ToGeoObject(this string geoJson, Type geoType) =>
        JsonConvert.DeserializeObject(geoJson, geoType, Options()) as GeoObject;

    public static GeoObject? ToGeoObject(this string geoJson) =>
        JsonConvert.DeserializeObject<GeoObject>(geoJson, Options());

    public static T? ToGeoObject<T>(this string geoJson) =>
        JsonConvert.DeserializeObject<T>(geoJson, Options());
}
