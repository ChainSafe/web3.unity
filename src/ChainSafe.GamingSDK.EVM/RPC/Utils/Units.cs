using System;
using System.Numerics;
using Nethereum.Util;

namespace Web3Unity.Scripts.Library.Ethers.Utils
{
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

        public static string FormatEther(BigInteger wei)
        {
            return FormatUnits(wei, 18);
        }

        public static BigInteger ParseEther(string ether)
        {
            return ParseUnits(ether, 18);
        }

        public static string FormatUnits(BigInteger value, int decimals)
        {
            return new BigDecimal(value, decimals * -1).ToString();
        }

        public static string FormatUnits(BigInteger value, string unitName)
        {
            var unitIndex = Array.IndexOf(Names, unitName);
            return unitIndex == -1 ? FormatUnits(value, 18) : FormatUnits(value, unitIndex * 3);
        }

        public static BigInteger ParseUnits(BigDecimal value, int decimals)
        {
            var bigDecimalFromUnit = new BigDecimal(decimals, 0);
            var conversion = value * bigDecimalFromUnit;
            return conversion.Floor().Mantissa;
        }

        public static BigInteger ParseUnits(string value, int decimals)
        {
            return ParseUnits(decimal.Parse(value), decimals);
        }

        public static BigInteger ParseUnits(string value, string unitName)
        {
            var unitIndex = Array.IndexOf(Names, unitName);
            return unitIndex == -1 ? ParseUnits(value, 18) : ParseUnits(value, unitIndex * 3);
        }
    }
}