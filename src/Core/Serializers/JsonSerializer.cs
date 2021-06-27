﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Core.Serializers {
    public class JsonSerializer : IJsonSerializer {
        public JsonSerializer() {
            var contractResolver = new DefaultContractResolver {
                NamingStrategy = new SnakeCaseNamingStrategy(),
            };
            _serializerSettings = new() {
                ContractResolver = contractResolver,
            };
        }

        private readonly JsonSerializerSettings _serializerSettings;

        public string Serialize<T>(T value) =>
            JsonConvert.SerializeObject(value, _serializerSettings);

        public T Deserialize<T>(string json) =>
            JsonConvert.DeserializeObject<T>(json);

        public JObject DeserializeDynamic(string json) =>
            JObject.Parse(json);
    }
}
