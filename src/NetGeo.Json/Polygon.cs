namespace NetGeo.Json;

public record Polygon : Geometry<double[][][]>, IGeoObject<double[][][]>
{
    public Polygon() : base(Array.Empty<double[][]>(), GeoType.Polygon)
    {
    }

    public Polygon(Geometry geometry) : base(geometry.BaseCoordinates as double[][][] ?? Array.Empty<double[][]>(), geometry.Type)
    {
        Bbox = geometry.Bbox;
    }
}
