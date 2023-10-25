using System;
using System.Numerics;
using Nethereum.Util;

namespace ChainSafe.Gaming.Evm.Utils
{
    /// <summary>
    /// A utility class for unit conversion regarding Ethereum's native currency.
    /// </summary>
    public class Units
    {
        private static readonly string[] Names = new string[]
        {
            "wei",
            "kwei",
            "mwei",
            "gwei",
            "szabo",
            "finney",
            "ether",
        };

        /// <summary>
        /// Converts Wei to Ether.
        /// </summary>
        /// <param name="wei">The Wei amount to be converted to Ether.</param>
        /// <returns>A string representing the converted Ether amount.</returns>
        public static string FormatEther(BigInteger wei)
        {
            return FormatUnits(wei, 18);
        }

        /// <summary>
        /// Converts Ether to Wei.
        /// </summary>
        /// <param name="ether">The Ether amount to be converted to Wei.</param>
        /// <returns>A BigInteger representing the converted Wei amount.</returns>
        public static BigInteger ParseEther(string ether)
        {
            return ParseUnits(ether, 18);
        }

        /// <summary>
        /// Formats units based on the given decimals.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="decimals">The number of decimals to be used for the formatting.</param>
        /// <returns>A string representing the formatted units.</returns>
        public static string FormatUnits(BigInteger value, int decimals)
        {
            return new BigDecimal(value, decimals * -1).ToString();
        }

        /// <summary>
        /// Formats units based on the given unit name.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="unitName">The name of the unit to be used for the formatting.</param>
        /// <returns>A string representing the formatted units.</returns>
        public static string FormatUnits(BigInteger value, string unitName)
        {
            var unitIndex = Array.IndexOf(Names, unitName);
            return unitIndex == -1 ? FormatUnits(value, 18) : FormatUnits(value, unitIndex * 3);
        }

        /// <summary>
        /// Parses units based on the given decimals.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        /// <param name="decimals">The number of decimals to be used for parsing.</param>
        /// <returns>A BigInteger representing the parsed units.</returns>
        public static BigInteger ParseUnits(BigDecimal value, int decimals)
        {
            var bigDecimalFromUnit = new BigDecimal(decimals, 0);
            var conversion = value * bigDecimalFromUnit;
            return conversion.Floor().Mantissa;
        }

        /// <summary>
        /// Parses units based on the given decimals.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        /// <param name="decimals">The number of decimals to be used for parsing.</param>
        /// <returns>A BigInteger representing the parsed units.</returns>
        public static BigInteger ParseUnits(string value, int decimals)
        {
            return ParseUnits(decimal.Parse(value), decimals);
        }

        /// <summary>
        /// Parses units based on the given unit name.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        /// <param name="unitName">The name of the unit to be used for parsing.</param>
        /// <returns>A BigInteger representing the parsed units.</returns>
        public static BigInteger ParseUnits(string value, string unitName)
        {
            var unitIndex = Array.IndexOf(Names, unitName);
            return unitIndex == -1 ? ParseUnits(value, 18) : ParseUnits(value, unitIndex * 3);
        }
    }
}