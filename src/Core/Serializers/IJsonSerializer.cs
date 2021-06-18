using Newtonsoft.Json.Linq;

namespace Core.Serializers {
    public interface IJsonSerializer {
        string Serialize<T>(T value);
        T Deserialize<T>(string json);
        JObject DeserializeDynamic(string json);
        T DeserializeObject<T>(JObject jObject);
    }
}
