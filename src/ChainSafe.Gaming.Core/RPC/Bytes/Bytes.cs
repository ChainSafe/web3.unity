using System;
using System.Text.RegularExpressions;

namespace Web3Unity.Scripts.Library.Ethers.Bytes
{
    public class Bytes
    {
        public static string Hexlify(byte[] value)
        {
            return "0x" + BitConverter.ToString(value).Replace("-", string.Empty);
        }

        public static string Hexlify(string value)
        {
            if (IsHexString(value))
            {
                return value;
            }

            return "0x" + value;
        }

        public static string HexZeroPad(string value, int lenght)
        {
            if (!IsHexString(value))
            {
                throw new Exception($"invalid hex string {value}");
            }

            if (value.Length > (2 * lenght) + 2)
            {
                throw new Exception($"value out of range {value}");
            }

            while (value.Length < (2 * lenght) + 2)
            {
                value = string.Format("0x0{0}", value.Substring(2));
            }

            return value;
        }

        public static string HexZeroPad(byte[] value, int lenght)
        {
            return HexZeroPad(Hexlify(value), lenght);
        }

        public static bool IsHexString(string value)
        {
            if (!new Regex("^0x[0-9a-fA-F]*$").IsMatch(value))
            {
                return false;
            }

            // if (length && value.length !== 2 + 2 * length) { return false; }
            return true;
        }
    }
}