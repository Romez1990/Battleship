using Newtonsoft.Json;
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
            JsonConvert.DeserializeObject<T>(json);
    }
}
