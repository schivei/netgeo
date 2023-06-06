namespace NetGeo.Json;

public record MultiPoint : Geometry<double[][]>, IGeoObject<double[][]>
{
    public MultiPoint() : base(Array.Empty<double[]>(), GeoType.MultiPoint)
    {
    }

    public MultiPoint(Geometry geometry) : base(geometry.BaseCoordinates as double[][] ?? Array.Empty<double[]>(), geometry.Type)
    {
        Bbox = geometry.Bbox;
    }
}
