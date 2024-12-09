using System;
using Newtonsoft.Json;
using Reown.Core.Common.Utils;

namespace Reown.AppKit.Unity.Utils
{
    /// <summary>
    /// Converts byte array to hex string and vice versa.
    /// </summary>
    /// <remarks>
    /// The default behavior of Newtonsoft.Json is to convert byte arrays to base64 strings.
    /// </remarks>
    public class ByteArrayJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
            {
                var hexString = (string)reader.Value;
                return hexString.HexToByteArray();
            }

            throw new JsonSerializationException("Expected byte array object value");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var byteArray = (byte[])value;
            var hexString = byteArray.ToHex(true);

            writer.WriteValue(hexString);
        }
    }
}