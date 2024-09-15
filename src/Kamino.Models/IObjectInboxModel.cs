namespace Kamino.Models;

[JsonConverter(typeof(ObjectInboxModelJsonConverter))]
public interface IObjectInboxModel { }
