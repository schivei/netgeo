namespace NetGeo.Json;

public record LineString : Geometry<double[][]>, IGeoObject<double[][]>
{
    public LineString() : base(Array.Empty<double[]>(), GeoType.LineString)
    {
    }

    public LineString(Geometry geometry) : base(geometry.BaseCoordinates as double[][] ?? Array.Empty<double[]>(), geometry.Type)
    {
        Bbox = geometry.Bbox;
    }
}
