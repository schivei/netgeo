namespace NetGeo.Json;

public interface IGeoObject<T>
{
    GeoType Type { get; set; }
    T Coordinates { get; set; }
    double[]? Bbox { get; set; }
}
