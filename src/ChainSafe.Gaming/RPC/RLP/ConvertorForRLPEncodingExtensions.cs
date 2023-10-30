using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ChainSafe.Gaming.Evm.RLP
{
    /// <summary>
    /// Provides set of static methods for the conversion operations required for RLP (Recursive Length Prefix) encoding .
    /// </summary>
    public static class ConvertorForRLPEncodingExtensions
    {
        /// <summary>
        /// Converts RLP-decoded bytes to BigInteger.
        /// </summary>
        /// <param name="bytes">Input byte array.</param>
        /// <returns>Returns BigInteger converted from the input byte array.</returns>
        public static BigInteger ToBigIntegerFromRLPDecoded(this byte[] bytes)
        {
            if (bytes == null)
            {
                return 0;
            }

            if (BitConverter.IsLittleEndian)
            {
                var listEncoded = bytes.ToList();
                listEncoded.Insert(0, 0x00);
                bytes = listEncoded.ToArray().Reverse().ToArray();
                return new BigInteger(bytes);
            }

            return new BigInteger(bytes);
        }

        /// <summary>
        /// Converts a BigInteger to bytes ready for RLP encoding.
        /// </summary>
        /// <param name="bigInteger">Input BigInteger.</param>
        /// <returns>Returns an array of bytes converted from the BigInteger.</returns>
        public static byte[] ToBytesForRLPEncoding(this BigInteger bigInteger)
        {
            return ToBytesFromNumber(bigInteger.ToByteArray());
        }

        /// <summary>
        /// Converts a integer to bytes ready for RLP encoding.
        /// </summary>
        /// <param name="number">Input integer.</param>
        /// <returns>Returns an array of bytes converted from the integer.</returns>
        public static byte[] ToBytesForRLPEncoding(this int number)
        {
            return ToBytesFromNumber(BitConverter.GetBytes(number));
        }

        /// <summary>
        /// Converts a long to bytes ready for RLP encoding.
        /// </summary>
        /// <param name="number">Input long.</param>
        /// <returns>Returns an array of bytes converted from the long.</returns>
        public static byte[] ToBytesForRLPEncoding(this long number)
        {
            return ToBytesFromNumber(BitConverter.GetBytes(number));
        }

        /// <summary>
        /// Converts a string to bytes ready for RLP encoding.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>Returns an array of bytes converted from the string.</returns>
        public static byte[] ToBytesForRLPEncoding(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// Converts a string array to an array of bytes ready for RLP encoding.
        /// </summary>
        /// <param name="strings">Input strings array.</param>
        /// <returns>Returns an array of array of bytes converted from the strings array.</returns>
        public static byte[][] ToBytesForRLPEncoding(this string[] strings)
        {
            var output = new List<byte[]>();
            foreach (var str in strings)
            {
                output.Add(str.ToBytesForRLPEncoding());
            }

            return output.ToArray();
        }

        /// <summary>
        /// Converts a bytes array to integer.
        /// </summary>
        /// <param name="bytes">Input bytes array.</param>
        /// <returns>Returns an integer converted from the bytes array.</returns>
        public static int ToIntFromRLPDecoded(this byte[] bytes)
        {
            return (int)ToBigIntegerFromRLPDecoded(bytes);
        }

        /// <summary>
        /// Converts a bytes array to long.
        /// </summary>
        /// <param name="bytes">Input bytes array.</param>
        /// <returns>Returns long converted from the bytes array.</returns>
        public static long ToLongFromRLPDecoded(this byte[] bytes)
        {
            return (long)ToBigIntegerFromRLPDecoded(bytes);
        }

        /// <summary>
        /// Converts a bytes array to string.
        /// </summary>
        /// <param name="bytes">Input bytes array.</param>
        /// <returns>Returns string converted from the bytes array.</returns>
        public static string ToStringFromRLPDecoded(this byte[] bytes)
        {
            if (bytes == null)
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Converts a bytes array that represents a number to a bytes array that represents the same number but with respect to Little Endian.
        /// </summary>
        /// <param name="bytes">Input bytes array.</param>
        /// <returns>Returns bytes array converted from the bytes array.</returns>
        public static byte[] ToBytesFromNumber(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            return TrimZeroBytes(bytes);
        }

        /// <summary>
        /// Removes zero bytes from the beginning of the input bytes array.
        /// </summary>
        /// <param name="bytes">Input bytes array.</param>
        /// <returns>New bytes array without zero bytes at the beginning.</returns>
        public static byte[] TrimZeroBytes(this byte[] bytes)
        {
            var trimmed = new List<byte>();
            var previousByteWasZero = true;

            for (var i = 0; i < bytes.Length; i++)
            {
                if (previousByteWasZero && bytes[i] == 0)
                {
                    continue;
                }

                previousByteWasZero = false;
                trimmed.Add(bytes[i]);
            }

            return trimmed.ToArray();
        }
    }
}