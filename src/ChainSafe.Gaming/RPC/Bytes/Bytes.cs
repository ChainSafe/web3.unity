using System;
using System.Text.RegularExpressions;

namespace ChainSafe.Gaming.Evm.Bytes
{
    /// <summary>
    /// Provides utility functions for manipulating and validating byte arrays and hexadecimal strings.
    /// </summary>
    public class Bytes
    {
        /// <summary>
        /// Converts a byte array into its hexadecimal representation with a "0x" prefix.
        /// </summary>
        /// <param name="value">Byte array to be converted.</param>
        /// <returns>Hexadecimal representation of the byte array with "0x" prefix.</returns>
        public static string Hexlify(byte[] value)
        {
            return "0x" + BitConverter.ToString(value).Replace("-", string.Empty);
        }

        /// <summary>
        /// Converts a string into its hexadecimal representation with a "0x" prefix.
        /// If the string is already in hex format, it is returned as is.
        /// </summary>
        /// <param name="value">String to be converted.</param>
        /// <returns>Hexadecimal representation of the string with "0x" prefix, or the original string if it's already in hex format.</returns>
        public static string Hexlify(string value)
        {
            if (IsHexString(value))
            {
                return value;
            }

            return "0x" + value;
        }

        /// <summary>
        /// Pads the provided hexadecimal string with zeros to meet the desired length.
        /// </summary>
        /// <param name="value">Hexadecimal string to be padded.</param>
        /// <param name="lenght">Desired length for the hexadecimal string representation excluding the "0x" prefix.</param>
        /// <returns>Hexadecimal string padded with zeros to the specified length.</returns>
        /// <exception cref="Exception">Thrown when the provided value is not a valid hex string or if the value's length exceeds the desired length.</exception>
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
                value = $"0x0{value.Substring(2)}";
            }

            return value;
        }

        /// <summary>
        /// Pads the provided byte array with zeros to meet the desired length in its hexadecimal representation.
        /// </summary>
        /// <param name="value">Byte array to be padded.</param>
        /// <param name="lenght">Desired length for the hexadecimal representation excluding the "0x" prefix.</param>
        /// <returns>Hexadecimal representation of the byte array, padded with zeros to the specified length.</returns>
        public static string HexZeroPad(byte[] value, int lenght)
        {
            return HexZeroPad(Hexlify(value), lenght);
        }

        /// <summary>
        /// Validates if the provided string is a valid hexadecimal string with a "0x" prefix.
        /// </summary>
        /// <param name="value">String to be validated.</param>
        /// <returns>True if the string is a valid hexadecimal string, otherwise false.</returns>
        public static bool IsHexString(string value)
        {
            if (!new Regex("^0x[0-9a-fA-F]*$").IsMatch(value))
            {
                return false;
            }

            return true;
        }
    }
}