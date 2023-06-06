using System.Globalization;
using Newtonsoft.Json;

namespace NetGeo.Json;

public static class GeoExtensions
{
    public static void SetDefaults()
    {
        var settings = JsonConvert.DefaultSettings?.Invoke() ?? new JsonSerializerSettings();

        if (settings.Converters.Any(x => x is GeoObjectConverter or CrsConverter))
            return;

        settings.Converters = new List<JsonConverter>(settings.Converters)
        {
            new GeoObjectConverter(),
            new CrsConverter()
        };

        settings.Culture = CultureInfo.InvariantCulture;

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
