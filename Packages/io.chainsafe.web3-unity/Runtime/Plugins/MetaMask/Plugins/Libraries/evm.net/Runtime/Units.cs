using System.Numerics;
using Nethereum.Util;

namespace evm.net
{
    public class Units
    {
        public BigDecimal Unit { get; }

        public BigDecimal Value { get; private set; } = 0;

        public static readonly Units Wei = new Units(new BigDecimal(1));

        public static readonly Units Gwei = new Units(BigDecimal.Parse("1000000000"));

        public static readonly Units Ether = new Units(BigDecimal.Parse("1000000000000000000"));

        private Units(BigDecimal unit)
        {
            Unit = unit;
        }

        public static Units WithDecimals(int decimals)
        {
            return WithDecimals(new BigInteger(decimals));
        }
        
        public static Units WithDecimals(BigInteger decimals)
        {
            var units = new Units(BigDecimal.Pow(10.0, (double)decimals));
            return units;
        }
        
        public static Units From(BigDecimal i, int decimals)
        {
            var units = new Units(BigDecimal.Pow(10.0, (double)decimals));
            return units.WithValue(i);
        }

        public static Units From(BigDecimal i, Units source)
        {
            var clonedSource = source.WithValue(i);

            return clonedSource;
        }

        public Units WithValue(BigDecimal value)
        {
            var newUnit = new Units(Unit);
            newUnit.Value = value;

            return newUnit;
        }

        public BigDecimal To(Units target)
        {
            return (Value) * (Unit / target.Unit);
        }
        
        public BigInteger ToBigInteger(Units target)
        {
            return BigInteger.Parse(((Value) * (Unit / target.Unit)).ToString());
        }
    }
}