using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using RestSharp.Deserializers;

namespace JenkinsRestClient
{
    internal class JsonNetDeserializer : IDeserializer
    {
        protected readonly JsonSerializerSettings SerializerSettings;
        private readonly JsonSerializer _serializer;

        public JsonNetDeserializer()
        {
            SerializerSettings = new JsonSerializerSettings();
            SerializerSettings.Converters.Add(new IsoDateTimeConverter());
            _serializer = JsonSerializer.Create(SerializerSettings);
        }

        #region IDeserializer Members

        public T Deserialize<T>(IRestResponse response)
        {
            using (var stringReader = new StringReader(response.Content))
            using (var reader = new JsonTextReader(stringReader))
            {
                return _serializer.Deserialize<T>(reader);
            }
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }

        #endregion
    }
}