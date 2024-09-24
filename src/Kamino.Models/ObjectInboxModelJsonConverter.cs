namespace Kamino.Models;

public class ObjectInboxModelJsonConverter : JsonConverter<IObjectInboxModel>
{
    public override ObjectInboxModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return new ObjectInboxModel()
            {
                Id = new Uri(reader.GetString()!)
            };
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            // See https://stackoverflow.com/a/60402592
            using var jsonDoc = JsonDocument.ParseValue(ref reader);

            return JsonSerializer.Deserialize<ObjectInboxModel>(jsonDoc.RootElement.ToString());
        }

        throw new InvalidOperationException("Unable to convert.");
    }

    public override void Write(Utf8JsonWriter writer, IObjectInboxModel value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
