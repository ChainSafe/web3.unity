using Nethereum.Hex.HexConvertors.Extensions;

namespace evm.net.Models
{
    public class HexString : IConvertableType
    {
        public string Value { get; set; }

        public HexString(string value)
        {
            Value = value;
        }

        public byte[] ToBytes()
        {
            return Value.HexToByteArray();
        }

        public static implicit operator string?(HexString d) => d?.Value;
        public static implicit operator HexString(string b) => new HexString(b);

        public override string ToString() => Value.StartsWith("0x") ? Value : $"0x{Value}";

        public object Convert()
        {
            return ToString();
        }
    }
}