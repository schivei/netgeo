using System.Text.Json.Serialization;
using NetGeo.Json.SystemText;

namespace NetGeo.Json;

public class Crs
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("properties")]
    public CrsProperties Properties { get; set; }
}
