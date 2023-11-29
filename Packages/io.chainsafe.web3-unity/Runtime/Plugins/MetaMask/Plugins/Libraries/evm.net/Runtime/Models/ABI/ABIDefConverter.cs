using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace evm.net.Models.ABI
{
    public class ABIDefConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var obj = JObject.Load(reader);
            
            if (obj["type"] == null) return serializer.Deserialize(reader, objectType);
            var typeName = obj["type"].ToString();

            ABIDefType type = (ABIDefType) Enum.Parse(typeof(ABIDefType), typeName, true);

            switch (type)
            {
                case ABIDefType.Constructor:
                case ABIDefType.Fallback:
                case ABIDefType.Function:
                case ABIDefType.Receive:
                    objectType = typeof(ABIFunction);
                    break;
                case ABIDefType.Event:
                    objectType = typeof(ABIEvent);
                    break;
                case ABIDefType.Error:
                    objectType = typeof(ABIError);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            ABIDef result = (existingValue as ABIDef ?? (ABIDef)serializer.ContractResolver.ResolveContract(objectType).DefaultCreator()); // Reuse existingValue if present
            // the structure of the object matches the first format,
            // so just deserialize it directly using the serializer
            using (var subReader = obj.CreateReader())
                serializer.Populate(subReader, result);

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ABIDef).IsAssignableFrom(objectType);
        }
    }
}