using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetGeo.Json.SystemText;

internal sealed class GeoObjectConverter : JsonConverter<GeoObject>
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsSubclassOf(typeof(GeoObject));

    public override GeoObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeof(Geometry).IsAssignableFrom(typeToConvert))
        {
            return ReadGeometry(ref reader, typeToConvert, options);
        }

        return ReadGeometries(ref reader, typeToConvert, options);
    }

    private static GeoObject ReadGeometries(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // can be GeoJsonFeature, GeoJsonFeatureCollection, GeoJsonGeometryCollection
        var GeoObject = (GeoObject)Activator.CreateInstance(typeToConvert);

        var el = JsonSerializer.Deserialize<JsonElement>(ref reader, options);

        if (el.TryGetProperty("bbox", out var bbox))
        {
            GeoObject.Bbox = JsonSerializer.Deserialize<double[]>(bbox.GetRawText(), options);
        }

        if (el.TryGetProperty("type", out var type))
        {
            GeoObject.Type = (GeoType)Enum.Parse(typeof(GeoType), type.GetString());
        }

        return GeoObject switch
        {
            FeatureCollection featureCollection => ReadFeatureCollection(el, featureCollection, options),
            Feature feature => ReadFeature(el, feature, options),
            GeometryCollection geometryCollection => ReadGeometryCollection(el, geometryCollection, options),
            _ when GeoObject.Type == GeoType.FeatureCollection => ReadFeatureCollection(el, new FeatureCollection(GeoObject), options),
            _ when GeoObject.Type == GeoType.Feature => ReadFeature(el, new Feature(GeoObject), options),
            _ when GeoObject.Type == GeoType.GeometryCollection => ReadGeometryCollection(el, new GeometryCollection(GeoObject), options),
            _ => throw new NotSupportedException($"Type {typeToConvert} is not supported.")
        };
    }

    private static GeometryCollection ReadGeometryCollection(JsonElement el, GeometryCollection geometryCollection, JsonSerializerOptions options)
    {
        if (el.TryGetProperty("geometries", out var geometries))
        {
            geometryCollection.Geometries = JsonSerializer.Deserialize<Geometry[]>(geometries.GetRawText(), options);
        }

        if (el.TryGetProperty("bbox", out var bbox))
        {
            geometryCollection.Bbox = JsonSerializer.Deserialize<double[]>(bbox.GetRawText(), options);
        }

        return geometryCollection;
    }

    private static Feature ReadFeature(JsonElement el, Feature feature, JsonSerializerOptions options)
    {
        if (el.TryGetProperty("geometry", out var geometry))
        {
            feature.Geometry = geometry.GetRawText().ToGeoObject<Geometry>();
        }

        if (el.TryGetProperty("properties", out var properties))
        {
            feature.Properties = JsonSerializer.Deserialize<Dictionary<string, string>>(properties.GetRawText(), options);
        }

        if (el.TryGetProperty("id", out var id))
        {
            feature.Id = id.GetString();
        }

        if (el.TryGetProperty("crs", out var crs))
        {
            feature.Crs = JsonSerializer.Deserialize<Crs>(crs.GetRawText());
        }

        if (el.TryGetProperty("bbox", out var bbox))
        {
            feature.Bbox = JsonSerializer.Deserialize<double[]>(bbox.GetRawText(), options);
        }

        return feature;
    }

    private static FeatureCollection ReadFeatureCollection(JsonElement el, FeatureCollection featureCollection, JsonSerializerOptions options)
    {
        if (el.TryGetProperty("features", out var features))
        {
            var feats = new List<Feature>();

            foreach (var arr in features.EnumerateArray())
            {
                feats.Add(arr.GetRawText().ToGeoObject<Feature>());
            }

            featureCollection.Features = [.. feats];
        }

        if (el.TryGetProperty("crs", out var crs))
        {
            featureCollection.Crs = JsonSerializer.Deserialize<Crs>(crs.GetRawText(), options);
        }

        if (el.TryGetProperty("bbox", out var bbox))
        {
            featureCollection.Bbox = JsonSerializer.Deserialize<double[]>(bbox.GetRawText(), options);
        }

        return featureCollection;
    }

    private static Geometry ReadGeometry(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // all other obejcts
        var geometry = (Geometry)Activator.CreateInstance(typeToConvert);

        var el = JsonSerializer.Deserialize<JsonElement>(ref reader, options);

        if (el.TryGetProperty("bbox", out var bbox))
        {
            geometry.Bbox = JsonSerializer.Deserialize<double[]>(bbox.GetRawText(), options);
        }

        if (el.TryGetProperty("type", out var type))
        {
            geometry.Type = (GeoType)Enum.Parse(typeof(GeoType), type.GetString());
        }

        if (el.TryGetProperty("coordinates", out var coordinates))
        {
            geometry = geometry switch
            {
                Point point => point with { Coordinates = GetCoordinates<double[]>(coordinates, options) },
                MultiPoint multiPoint => multiPoint with { Coordinates = GetCoordinates<double[][]>(coordinates, options) },
                LineString lineString => lineString with { Coordinates = GetCoordinates<double[][]>(coordinates, options) },
                MultiLineString multiLineString => multiLineString with { Coordinates = GetCoordinates<double[][][]>(coordinates, options) },
                Polygon polygon => polygon with { Coordinates = GetCoordinates<double[][][]>(coordinates, options) },
                MultiPolygon multiPolygon => multiPolygon with { Coordinates = GetCoordinates<double[][][][]>(coordinates, options) },
                _ when geometry.Type == GeoType.Point => new Point(geometry) with { Coordinates = GetCoordinates<double[]>(coordinates, options) },
                _ when geometry.Type == GeoType.MultiPoint => new MultiPoint(geometry) with { Coordinates = GetCoordinates<double[][]>(coordinates, options) },
                _ when geometry.Type == GeoType.LineString => new LineString(geometry) with { Coordinates = GetCoordinates<double[][]>(coordinates, options) },
                _ when geometry.Type == GeoType.MultiLineString => new MultiLineString(geometry) with { Coordinates = GetCoordinates<double[][][]>(coordinates, options) },
                _ when geometry.Type == GeoType.Polygon => new Polygon(geometry) with { Coordinates = GetCoordinates<double[][][]>(coordinates, options) },
                _ when geometry.Type == GeoType.MultiPolygon => new MultiPolygon(geometry) with { Coordinates = GetCoordinates<double[][][][]>(coordinates, options) },
                _ => throw new NotSupportedException($"Type {typeToConvert} is not supported.")
            };
        }

        return geometry;
    }

    private static T GetCoordinates<T>(JsonElement coordinates, JsonSerializerOptions options)
    {
        // coordinates to T
        return JsonSerializer.Deserialize<T>(coordinates.GetRawText(), options);
    }

    public override void Write(Utf8JsonWriter writer, GeoObject value, JsonSerializerOptions options)
    {
        if (value is Geometry geom)
        {
            WriteGeometry(writer, geom, options);
        }
        else
        {
            WriteGeometries(writer, value, options);
        }
    }

    private static void WriteGeometry(Utf8JsonWriter writer, Geometry geom, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("type", Enum.GetName(geom.Type));

        if (geom.Bbox is not null)
        {
            writer.WritePropertyName("bbox");
            JsonSerializer.Serialize(writer, geom.Bbox, options);
        }

        writer.WritePropertyName("coordinates");
        JsonSerializer.Serialize(writer, geom.BaseCoordinates, options);

        writer.WriteEndObject();
    }

    private static void WriteGeometries(Utf8JsonWriter writer, GeoObject geo, JsonSerializerOptions options)
    {
        switch (geo)
        {
            case FeatureCollection featureCollection:
                WriteFeatureCollection(writer, featureCollection, options);
                break;

            case Feature feature:
                WriteFeature(writer, feature, options);
                break;

            case GeometryCollection geometryCollection:
                WriteGeometryCollection(writer, geometryCollection, options);
                break;
        }
    }

    private static void WriteFeatureCollection(Utf8JsonWriter writer, FeatureCollection featureCollection, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("type", Enum.GetName(featureCollection.Type));

        if (options.Converters.All(x => x is not GeoObjectConverter))
        {
            options.Converters.Add(new GeoObjectConverter());
        }

        if (options.Converters.All(x => x is not GeoObjectConverter))
        {
            options.Converters.Add(new GeoObjectConverter());
        }

        if (featureCollection.Bbox is not null)
        {
            writer.WritePropertyName("bbox");
            JsonSerializer.Serialize(writer, featureCollection.Bbox, options);
        }

        if (featureCollection.Features is not null)
        {
            writer.WritePropertyName("features");
            writer.WriteStartArray();

            foreach (var feature in featureCollection.Features)
            {
                writer.WriteRawValue(feature.ToGeoJson());
            }

            writer.WriteEndArray();
        }

        if (featureCollection.Crs is not null && featureCollection.Crs.Properties.Name != "urn:ogc:def:crs:OGC:1.3:CRS84")
        {
            writer.WritePropertyName("crs");
            JsonSerializer.Serialize(writer, featureCollection.Crs, options);
        }

        writer.WriteEndObject();
    }

    private static void WriteFeature(Utf8JsonWriter writer, Feature feature, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("type", Enum.GetName(feature.Type));

        if (options.Converters.All(x => x is not GeoObjectConverter))
        {
            options.Converters.Add(new GeoObjectConverter());
        }

        if (feature.Bbox is not null)
        {
            writer.WritePropertyName("bbox");
            JsonSerializer.Serialize(writer, feature.Bbox, options);
        }

        if (feature.Geometry is not null)
        {
            writer.WritePropertyName("geometry");
            writer.WriteRawValue(feature.Geometry.ToGeoJson());
        }

        if (feature.Properties is not null)
        {
            writer.WritePropertyName("properties");
            JsonSerializer.Serialize(writer, feature.Properties, options);
        }

        if (feature.Id is not null)
        {
            writer.WritePropertyName("id");
            JsonSerializer.Serialize(writer, feature.Id, options);
        }

        if (feature.Crs is not null && feature.Crs.Properties.Name != "urn:ogc:def:crs:OGC:1.3:CRS84")
        {
            writer.WritePropertyName("crs");
            JsonSerializer.Serialize(writer, feature.Crs, options);
        }

        writer.WriteEndObject();
    }

    private static void WriteGeometryCollection(Utf8JsonWriter writer, GeometryCollection geometryCollection, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (options.Converters.All(x => x is not GeoObjectConverter))
        {
            options.Converters.Add(new GeoObjectConverter());
        }

        writer.WriteString("type", Enum.GetName(geometryCollection.Type));

        if (geometryCollection.Bbox is not null)
        {
            writer.WritePropertyName("bbox");
            JsonSerializer.Serialize(writer, geometryCollection.Bbox, options);
        }

        if (geometryCollection.Geometries is not null)
        {
            writer.WritePropertyName("geometries");

            writer.WriteStartArray();

            foreach (var geometry in geometryCollection.Geometries)
            {
                writer.WriteRawValue(geometry.ToGeoJson());
            }

            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}
