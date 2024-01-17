using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Modules.HumanResources.Domain.CustomConverters;

public class CustomTimespanJsonConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "ticks")
        {
            throw new JsonException();
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException();
        }

        long ticks = reader.GetInt64();

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException();
        }

        return new TimeSpan(ticks); 
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("ticks", value.Ticks);
        writer.WriteEndObject();
    } 
}