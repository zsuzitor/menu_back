using System.Text.Json;


namespace WEB.Common.Models.Returns
{

    public interface IApiSerializer
    {
        string ContentType { get; }
        string Serialize<T>(T obj);
    }


    public sealed class JsonApiSerializer : IApiSerializer
    {
        public string ContentType => "application/json";

        public string Serialize<T>(T data)
        {
            return JsonSerializer.Serialize(data);
        }
    }
}
