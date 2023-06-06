namespace NetGeo.Json;

public record Point : Geometry<double[]>, IGeoObject<double[]>
{
    public Point() : base(Array.Empty<double>(), GeoType.Point)
    {
    }

    public Point(Geometry geometry) : base(geometry.BaseCoordinates as double[] ?? Array.Empty<double>(), geometry.Type)
    {
        Bbox = geometry.Bbox;
    }
}
