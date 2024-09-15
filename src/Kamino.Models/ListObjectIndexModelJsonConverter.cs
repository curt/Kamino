namespace Kamino.Models;

public class ListObjectIndexModelJsonConverter : JsonConverter<IListObjectInboxModel>
{
    public override ListObjectInboxModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ListObjectInboxModel list = [];

        if (reader.TokenType == JsonTokenType.String)
        {
            JsonReadString(ref reader, list);
            return list;
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            JsonReadObject(ref reader, list);
            return list;
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return list;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    JsonReadString(ref reader, list);
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    JsonReadObject(ref reader, list);
                }
            }
        }

        throw new InvalidOperationException("Unable to convert.");
    }

    private static void JsonReadString(ref Utf8JsonReader reader, ListObjectInboxModel list)
    {
        string? read = reader.GetString();

        if (read != null)
        {
            list.Add(new() { Id = read });
        }
    }

    private static void JsonReadObject(ref Utf8JsonReader reader, ListObjectInboxModel list)
    {
        // See https://stackoverflow.com/a/60402592
        using var jsonDoc = JsonDocument.ParseValue(ref reader);

        ObjectInboxModel? read = JsonSerializer.Deserialize<ObjectInboxModel>(jsonDoc.RootElement.ToString());

        if (read != null)
        {
            list.Add(read);
        }
    }

    public override void Write(Utf8JsonWriter writer, IListObjectInboxModel value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
