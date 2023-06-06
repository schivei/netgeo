namespace NetGeo.Json;

public record MultiLineString : Geometry<double[][][]>, IGeoObject<double[][][]>
{
    public MultiLineString() : base(Array.Empty<double[][]>(), GeoType.MultiLineString)
    {
    }

    public MultiLineString(Geometry geometry) : base(geometry.BaseCoordinates as double[][][] ?? Array.Empty<double[][]>(), geometry.Type)
    {
        Bbox = geometry.Bbox;
    }
}
