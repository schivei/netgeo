namespace NetGeo.Json;

public class CrsProperties : Dictionary<string, string>
{
    public CrsProperties()
    {
    }

    public CrsProperties(IDictionary<string, string> dictionary) : base(dictionary)
    {
    }

    public CrsProperties(IEqualityComparer<string> comparer) : base(comparer)
    {
    }

    public CrsProperties(int capacity) : base(capacity)
    {
    }

    public CrsProperties(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
    {
    }

    public CrsProperties(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
    {
    }

    public string Name
    {
        get => this["name"] ?? string.Empty;
        set => this["name"] = value;
    }
}
