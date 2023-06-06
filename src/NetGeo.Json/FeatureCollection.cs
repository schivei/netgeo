namespace NetGeo.Json;

public record FeatureCollection : GeoObject
{
    public Feature[] Features { get; set; } = Array.Empty<Feature>();

    public Crs? Crs { get; set; }

    public FeatureCollection() : base(GeoType.FeatureCollection)
    {
        Crs = new Crs
        {
            Type = "name",
            Properties = new CrsProperties
            {
                Name = "urn:ogc:def:crs:OGC:1.3:CRS84"
            }
        };
    }

    public FeatureCollection(GeoObject geoJsonObject)
    {
        Type = geoJsonObject.Type;
        Bbox = geoJsonObject.Bbox;

        Crs = new Crs
        {
            Type = "name",
            Properties = new CrsProperties
            {
                Name = "urn:ogc:def:crs:OGC:1.3:CRS84"
            }
        };
    }
}
