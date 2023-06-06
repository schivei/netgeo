namespace NetGeo.Json;

public record GeoObject
{
    public GeoType Type { get; set; }

    public double[]? Bbox { get; set; }

    protected GeoObject(GeoType type)
    {
        Type = type;
    }

    public GeoObject()
    {
    }
}
