using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetGeo.Json;

internal sealed class GeoObjectConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) =>
        objectType.IsSubclassOf(typeof(GeoObject));

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (typeof(Geometry).IsAssignableFrom(objectType))
        {
            return ReadGeometry(reader, objectType, existingValue, serializer);
        }

        return ReadGeometries(reader, objectType, existingValue, serializer);
    }

    private static object? ReadGeometries(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var geoObject = (GeoObject)Activator.CreateInstance(objectType);

        var el = serializer.Deserialize<JObject>(reader);

        if (el.TryGetValue("bbox", out var bbox))
        {
            geoObject.Bbox = bbox.ToObject<double[]>();
        }

        if (el.TryGetValue("type", out var type))
        {
            geoObject.Type = type.ToObject<GeoType>();
        }

        return geoObject switch
        {
            FeatureCollection featureCollection => ReadFeatureCollection(el, featureCollection, serializer),
            Feature feature => ReadFeature(el, feature, serializer),
            GeometryCollection geometryCollection => ReadGeometryCollection(el, geometryCollection, serializer),
            _ when geoObject.Type == GeoType.FeatureCollection => ReadFeatureCollection(el, new FeatureCollection(geoObject), serializer),
            _ when geoObject.Type == GeoType.Feature => ReadFeature(el, new Feature(geoObject), serializer),
            _ when geoObject.Type == GeoType.GeometryCollection => ReadGeometryCollection(el, new GeometryCollection(geoObject), serializer),
            _ => existingValue ?? throw new NotSupportedException($"Type {objectType} is not supported.")
        };
    }

    private static object? ReadGeometryCollection(JObject el, GeometryCollection geometryCollection, JsonSerializer serializer)
    {
        if (el.TryGetValue("geometries", out var geometries))
        {
            geometryCollection.Geometries = geometries.ToObject<Geometry[]>(serializer);
        }

        return geometryCollection;
    }

    private static object? ReadFeature(JObject el, Feature feature, JsonSerializer serializer)
    {
        if (el.TryGetValue("geometry", out var geometry))
        {
            feature.Geometry = geometry.ToObject<Geometry>(serializer);
        }

        if (el.TryGetValue("properties", out var properties))
        {
            feature.Properties = properties.ToObject<Dictionary<string, string>>(serializer);
        }

        if (el.TryGetValue("id", out var id))
        {
            feature.Id = id.ToObject<string>(serializer);
        }

        if (el.TryGetValue("bbox", out var bbox))
        {
            feature.Bbox = bbox.ToObject<double[]>();
        }

        if (el.TryGetValue("crs", out var crs))
        {
            feature.Crs = crs.ToObject<Crs>(serializer);
        }

        return feature;
    }

    private static object? ReadFeatureCollection(JObject el, FeatureCollection featureCollection, JsonSerializer serializer)
    {
        if (el.TryGetValue("features", out var features))
        {
            featureCollection.Features = features.ToObject<Feature[]>(serializer);
        }

        if (el.TryGetValue("bbox", out var bbox))
        {
            featureCollection.Bbox = bbox.ToObject<double[]>();
        }

        if (el.TryGetValue("crs", out var crs))
        {
            featureCollection.Crs = crs.ToObject<Crs>(serializer);
        }

        return featureCollection;
    }

    private static object? ReadGeometry(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var geometry = (Geometry)Activator.CreateInstance(objectType);

        var el = serializer.Deserialize<JObject>(reader);

        if (el.TryGetValue("bbox", out var bbox))
        {
            geometry.Bbox = bbox.ToObject<double[]>();
        }

        if (el.TryGetValue("type", out var type))
        {
            geometry.Type = type.ToObject<GeoType>();
        }

        return geometry switch
        {
            Point point => ReadPoint(el, point, serializer),
            MultiPoint multiPoint => ReadMultiPoint(el, multiPoint, serializer),
            LineString lineString => ReadLineString(el, lineString, serializer),
            MultiLineString multiLineString => ReadMultiLineString(el, multiLineString, serializer),
            Polygon polygon => ReadPolygon(el, polygon, serializer),
            MultiPolygon multiPolygon => ReadMultiPolygon(el, multiPolygon, serializer),
            _ when geometry.Type == GeoType.Point => ReadPoint(el, new Point(geometry), serializer),
            _ when geometry.Type == GeoType.MultiPoint => ReadMultiPoint(el, new MultiPoint(geometry), serializer),
            _ when geometry.Type == GeoType.LineString => ReadLineString(el, new LineString(geometry), serializer),
            _ when geometry.Type == GeoType.MultiLineString => ReadMultiLineString(el, new MultiLineString(geometry), serializer),
            _ when geometry.Type == GeoType.Polygon => ReadPolygon(el, new Polygon(geometry), serializer),
            _ when geometry.Type == GeoType.MultiPolygon => ReadMultiPolygon(el, new MultiPolygon(geometry), serializer),
            _ => existingValue ?? throw new NotSupportedException($"Type {objectType} is not supported.")
        };
    }

    private static object? ReadMultiPolygon(JObject el, MultiPolygon multiPolygon, JsonSerializer serializer)
    {
        if (el.TryGetValue("coordinates", out var coordinates))
        {
            multiPolygon.Coordinates = coordinates.ToObject<double[][][][]>(serializer);
        }

        return multiPolygon;
    }

    private static object? ReadPolygon(JObject el, Polygon polygon, JsonSerializer serializer)
    {
        if (el.TryGetValue("coordinates", out var coordinates))
        {
            polygon.Coordinates = coordinates.ToObject<double[][][]>(serializer);
        }

        return polygon;
    }

    private static object? ReadMultiLineString(JObject el, MultiLineString multiLineString, JsonSerializer serializer)
    {
        if (el.TryGetValue("coordinates", out var coordinates))
        {
            multiLineString.Coordinates = coordinates.ToObject<double[][][]>(serializer);
        }

        return multiLineString;
    }

    private static object? ReadLineString(JObject el, LineString lineString, JsonSerializer serializer)
    {
        if (el.TryGetValue("coordinates", out var coordinates))
        {
            lineString.Coordinates = coordinates.ToObject<double[][]>(serializer);
        }

        return lineString;
    }

    private static object? ReadMultiPoint(JObject el, MultiPoint multiPoint, JsonSerializer serializer)
    {
        if (el.TryGetValue("coordinates", out var coordinates))
        {
            multiPoint.Coordinates = coordinates.ToObject<double[][]>(serializer);
        }

        return multiPoint;
    }

    private static object? ReadPoint(JObject el, Point point, JsonSerializer serializer)
    {
        if (el.TryGetValue("coordinates", out var coordinates))
        {
            point.Coordinates = coordinates.ToObject<double[]>(serializer);
        }

        return point;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is Geometry geom)
        {
            WriteGeometry(writer, geom, serializer);
        }
        else
        {
            WriteGeometries(writer, value, serializer);
        }
    }

    private static void WriteGeometry(JsonWriter writer, Geometry geom, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("type");
        writer.WriteValue(geom.Type.ToString());

        if (geom.Bbox != null)
        {
            writer.WritePropertyName("bbox");
            serializer.Serialize(writer, geom.Bbox);
        }

        writer.WritePropertyName("coordinates");
        serializer.Serialize(writer, geom.BaseCoordinates);

        writer.WriteEndObject();
    }

    private static void WriteGeometries(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        switch (value)
        {
            case FeatureCollection featureCollection:
                WriteFeatureCollection(writer, featureCollection, serializer);
                break;

            case Feature feature:
                WriteFeature(writer, feature, serializer);
                break;

            case GeometryCollection geometryCollection:
                WriteGeometryCollection(writer, geometryCollection, serializer);
                break;
        }
    }

    private static void WriteGeometryCollection(JsonWriter writer, GeometryCollection geometryCollection, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("type");
        writer.WriteValue(geometryCollection.Type.ToString());

        if (geometryCollection.Bbox != null)
        {
            writer.WritePropertyName("bbox");
            serializer.Serialize(writer, geometryCollection.Bbox);
        }

        if (geometryCollection.Geometries != null)
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

    private static void WriteFeatureCollection(JsonWriter writer, FeatureCollection featureCollection, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("type");
        writer.WriteValue(featureCollection.Type.ToString());

        if (featureCollection.Bbox != null)
        {
            writer.WritePropertyName("bbox");
            serializer.Serialize(writer, featureCollection.Bbox);
        }

        if (featureCollection.Features != null)
        {
            writer.WritePropertyName("features");
            writer.WriteStartArray();

            foreach (var feature in featureCollection.Features)
            {
                writer.WriteRawValue(feature.ToGeoJson());
            }

            writer.WriteEndArray();
        }

        if (featureCollection.Crs != null && featureCollection.Crs.Properties.Name != "urn:ogc:def:crs:OGC:1.3:CRS84")
        {
            writer.WritePropertyName("crs");
            serializer.Serialize(writer, featureCollection.Crs);
        }

        writer.WriteEndObject();
    }

    private static void WriteFeature(JsonWriter writer, Feature feature, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("type");
        writer.WriteValue(feature.Type.ToString());

        if (feature.Bbox != null)
        {
            writer.WritePropertyName("bbox");
            serializer.Serialize(writer, feature.Bbox);
        }

        if (feature.Geometry != null)
        {
            writer.WritePropertyName("geometry");
            writer.WriteRawValue(feature.Geometry.ToGeoJson());
        }

        if (feature.Properties != null)
        {
            writer.WritePropertyName("properties");
            serializer.Serialize(writer, feature.Properties);
        }

        if (feature.Id != null)
        {
            writer.WritePropertyName("id");
            serializer.Serialize(writer, feature.Id);
        }

        if (feature.Crs?.Properties?.Name != null && feature.Crs.Properties.Name != "urn:ogc:def:crs:OGC:1.3:CRS84")
        {
            writer.WritePropertyName("crs");
            serializer.Serialize(writer, feature.Crs);
        }

        writer.WriteEndObject();
    }
}
