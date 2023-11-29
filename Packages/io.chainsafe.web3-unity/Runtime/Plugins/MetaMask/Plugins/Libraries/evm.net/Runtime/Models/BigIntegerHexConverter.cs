using System;
using System.Globalization;
using System.Numerics;
using Newtonsoft.Json;

namespace evm.net.Models
{
    public class BigIntegerHexConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue("0x" + ((BigInteger)value).ToString("X"));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string str = reader.Value?.ToString();
                if (str == null)
                {
                    str = reader.ReadAsString();
                    if (str == null)
                        throw new JsonSerializationException();
                }

                if (str == "0x")
                    return BigInteger.Zero;
                
                if (str.StartsWith("0x"))
                    return BigInteger.Parse(str.Substring(2), NumberStyles.AllowHexSpecifier);
                return BigInteger.Parse(str);
            } 
            else if (reader.TokenType == JsonToken.Integer)
            {
                var num = reader.ReadAsInt32();
                if (num == null)
                    throw new JsonSerializationException();
                
                return new BigInteger((int)num);
            }

            throw new JsonSerializationException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BigInteger);
        }
    }
}