using System.Globalization;

namespace NetGeo.Json;

public class Position
{
    public Position() { }

    public Position(double longitude, double latitude, double? altitude)
    {
        Longitude = longitude;
        Latitude = latitude;
        Altitude = altitude;
    }

    public Position(params double[] coordinates)
    {
        if (coordinates.Length < 2 || coordinates.Length > 3)
            throw new ArgumentException("Coordinates must have 2 or 3 values", nameof(coordinates));

        Longitude = coordinates[0];
        Latitude = coordinates[1];

        if (coordinates.Length > 2)
            Altitude = coordinates[2];
    }

    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public double? Altitude { get; set; }

    public double[] ToCoordinates() =>
        Altitude.HasValue ? new[] { Longitude, Latitude, Altitude.Value } : new[] { Longitude, Latitude };

    public override string ToString() =>
        $"[{string.Join(",", ToCoordinates().Select(x => x.ToString(CultureInfo.InvariantCulture)))}]";

    public static implicit operator Position(double[] coordinates) =>
        new(coordinates);

    public static implicit operator double[](Position position) =>
        position.ToCoordinates();

    public static implicit operator Position(string coordinates) =>
        new(coordinates[1..^1].Split(',').Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray());

    public static implicit operator string(Position position) =>
        position.ToString();

    // == operator
    public static bool operator ==(Position left, Position right) =>
        left.ToCoordinates().SequenceEqual(right.ToCoordinates());

    public static bool operator ==(Position left, double[] right) =>
        left.ToCoordinates().SequenceEqual(right);

    public static bool operator ==(double[] left, Position right) =>
        left.SequenceEqual(right.ToCoordinates());

    // != operator
    public static bool operator !=(Position left, Position right) =>
        !(left == right);

    public static bool operator !=(Position left, double[] right) =>
        !(left == right);

    public static bool operator !=(double[] left, Position right) =>
        !(left == right);

    public override bool Equals(object obj) =>
        obj switch
        {
            Position position => this == position,
            double[] coordinates => this == coordinates,
            _ => false
        };

    public override int GetHashCode()
    {
        var hashCode = -1937169414;
        hashCode = (hashCode * -1521134295) + EqualityComparer<double>.Default.GetHashCode(Latitude);
        hashCode = (hashCode * -1521134295) + EqualityComparer<double>.Default.GetHashCode(Longitude);
        if (Altitude.HasValue)
            hashCode = (hashCode * -1521134295) + EqualityComparer<double?>.Default.GetHashCode(Altitude);
        return hashCode;
    }
}
