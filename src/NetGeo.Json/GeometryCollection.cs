namespace NetGeo.Json;

public record GeometryCollection : GeoObject
{
    public GeometryCollection() : base(GeoType.GeometryCollection)
    {
        Geometries = Array.Empty<Geometry>();
    }

    public GeometryCollection(GeoObject geoJsonObject)
    {
        Type = geoJsonObject.Type;
        Bbox = geoJsonObject.Bbox;

        Geometries = Array.Empty<Geometry>();
    }

    public Geometry[] Geometries { get; set; }
}
