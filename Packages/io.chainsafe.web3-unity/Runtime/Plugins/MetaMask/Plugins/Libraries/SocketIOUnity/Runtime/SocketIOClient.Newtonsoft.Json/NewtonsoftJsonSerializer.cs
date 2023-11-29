using System;
using System.Collections.Generic;

using MetaMask.SocketIOClient.JsonSerializer;

using Newtonsoft.Json;

namespace MetaMask.SocketIOClient.Newtonsoft.Json
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        public Func<JsonSerializerSettings> JsonSerializerOptions { get; }

        public JsonSerializeResult Serialize(object[] data)
        {
            var converter = new ByteArrayConverter();
            var settings = GetOptions();
            settings.Converters.Add(converter);
            string json = JsonConvert.SerializeObject(data, settings);
            return new JsonSerializeResult
            {
                Json = json,
                Bytes = converter.Bytes
            };
        }

        public T Deserialize<T>(string json)
        {
            var settings = GetOptions();
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public T Deserialize<T>(string json, IList<byte[]> bytes)
        {
            var converter = new ByteArrayConverter();
            converter.Bytes.AddRange(bytes);
            var settings = GetOptions();
            settings.Converters.Add(converter);
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        private JsonSerializerSettings GetOptions()
        {
            JsonSerializerSettings options;
            if (OptionsProvider != null)
            {
                options = OptionsProvider();
            }
            else
            {
                options = new JsonSerializerSettings();
            }
            if (options == null)
            {
                options = new JsonSerializerSettings();
            }
            return options;
        }

        //[Obsolete("Use Options instead.")]
        //public virtual JsonSerializerSettings CreateOptions()
        //{
        //    return new JsonSerializerSettings();
        //}

        public Func<JsonSerializerSettings> OptionsProvider { get; set; }
    }
}
