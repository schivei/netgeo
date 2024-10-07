using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetGeo.Json;

internal sealed class CrsConverter : JsonConverter<Crs>
{
    public override Crs? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = new Crs();

        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();

                    if (propertyName == "type")
                    {
                        reader.Read();
                        value.Type = reader.GetString() ?? string.Empty;
                    }
                    else if (propertyName == "properties")
                    {
                        reader.Read();
                        var kvp = JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options);
                        value.Properties = new(kvp);
                    }
                }
            }
        }

        return value;
    }

    public override void Write(Utf8JsonWriter writer, Crs value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        writer.WritePropertyName("type");
        writer.WriteStringValue(value.Type);

        writer.WritePropertyName("properties");
        JsonSerializer.Serialize(writer, value.Properties.ToDictionary(x => x.Key, x => x.Value));

        writer.WriteEndObject();
    }
}
