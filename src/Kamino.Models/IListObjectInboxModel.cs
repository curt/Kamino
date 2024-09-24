namespace Kamino.Models;

[JsonConverter(typeof(ListObjectIndexModelJsonConverter))]
public interface IListObjectInboxModel : IEnumerable<ObjectInboxModel> { }
