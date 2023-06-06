namespace NetGeo.Json;

public record Geometry<T> : Geometry
{
    protected Geometry(T empty, GeoType type) : base(empty, type) { }

    public T Coordinates
    {
        get => (T)BaseCoordinates;
        set => BaseCoordinates = value;
    }
}
