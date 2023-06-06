namespace NetGeo.Json;

public record MultiPolygon : Geometry<double[][][][]>, IGeoObject<double[][][][]>
{
    public MultiPolygon() : base(Array.Empty<double[][][]>(), GeoType.MultiPolygon)
    {
    }

    public MultiPolygon(Geometry geometry) : base(geometry.BaseCoordinates as double[][][][] ?? Array.Empty<double[][][]>(), geometry.Type)
    {
        Bbox = geometry.Bbox;
    }
}
