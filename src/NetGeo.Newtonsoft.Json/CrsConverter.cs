using Newtonsoft.Json;

namespace NetGeo.Json;

internal sealed class CrsConverter : JsonConverter<Crs>
{
    public override Crs? ReadJson(JsonReader reader, Type objectType, Crs? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = existingValue ?? new Crs();

        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonToken.StartObject)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = reader.Value as string;

                    if (propertyName == "type")
                    {
                        reader.Read();
                        value.Type = reader.Value as string ?? string.Empty;
                    }
                    else if (propertyName == "properties")
                    {
                        reader.Read();
                        value.Properties = new(serializer.Deserialize<Dictionary<string, string>>(reader));
                    }
                }
            }
        }

        return value;
    }

    public override void WriteJson(JsonWriter writer, Crs? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();

        writer.WritePropertyName("type");
        writer.WriteValue(value.Type);

        writer.WritePropertyName("properties");
        writer.WriteStartObject();

        foreach (var kvp in value.Properties)
        {
            writer.WritePropertyName(kvp.Key);
            writer.WriteValue(kvp.Value);
        }

        writer.WriteEndObject();

        writer.WriteEndObject();
    }
}
