namespace NetGeo.Json;

public record Feature : GeoObject
{
    public Geometry Geometry { get; set; }

    public Dictionary<string, string>? Properties { get; set; }

    public string? Id { get; set; }

    public Crs? Crs { get; set; }

    public Feature() : base(GeoType.Feature)
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

    public Feature(GeoObject geoJsonObject)
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
