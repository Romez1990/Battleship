using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Core.Serializers {
    public class JsonSerializer : IJsonSerializer {
        public JsonSerializer() {
            var contractResolver = new DefaultContractResolver {
                NamingStrategy = new SnakeCaseNamingStrategy(),
            };
            _settings = new() {
                ContractResolver = contractResolver,
            };
        }

        private readonly JsonSerializerSettings _settings;

        public string Serialize<T>(T value) =>
            JsonConvert.SerializeObject(value, _settings);

        public T Deserialize<T>(string json) =>
            JsonConvert.DeserializeObject<T>(json, _settings);

        public JObject DeserializeDynamic(string json) =>
            (JObject)JsonConvert.DeserializeObject(json, _settings);

        public T DeserializeObject<T>(JObject jObject) =>
            JsonConvert.DeserializeObject<T>(jObject.ToString(), _settings);
    }
}
