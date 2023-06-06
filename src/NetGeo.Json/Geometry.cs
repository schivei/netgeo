namespace NetGeo.Json;

public record Geometry : GeoObject, IEqualityComparer<Geometry>, IEquatable<Geometry>
{
    protected Geometry(object empty, GeoType type) : base(type) =>
        _coordinates = empty;

    public Geometry() { }

    protected object _coordinates;

    public object BaseCoordinates
    {
        get => _coordinates;
        set => _coordinates = value;
    }

    public virtual bool Equals(Geometry other)
    {
        return Equals(this, other);
    }

    public bool Equals(Geometry x, Geometry y) =>
        x is not null && y is not null && x.Type == y.Type && x._coordinates?.Equals(y._coordinates) == true;

    public int GetHashCode(Geometry obj)
    {
        return obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        var hashCode = -1937169414;
        hashCode = (hashCode * -1521134295) + Type.GetHashCode();
        hashCode = (hashCode * -1521134295) + EqualityComparer<object>.Default.GetHashCode(_coordinates);
        return hashCode;
    }
}
