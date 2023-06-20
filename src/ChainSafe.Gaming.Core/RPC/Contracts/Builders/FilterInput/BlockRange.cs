using System;
using System.Numerics;
using Nethereum.Hex.HexTypes;

namespace Web3Unity.Scripts.Library.Ethers.Contracts.Builders.FilterInput
{
    public readonly struct BlockRange :
        IEquatable<BlockRange>
    {
        private readonly int hashCode;

        public BlockRange(ulong from, ulong to)
            : this(new BigInteger(from), new BigInteger(to))
        {
        }

        public BlockRange(BigInteger from, BigInteger to)
            : this(new HexBigInteger(from), new HexBigInteger(to))
        {
        }

        public BlockRange(HexBigInteger from, HexBigInteger to)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            BlockCount = (To.Value - From.Value) + 1;
            hashCode = new { From, To }.GetHashCode();
        }

        public HexBigInteger From { get; }

        public HexBigInteger To { get; }

        public BigInteger BlockCount { get; }

        public static bool operator ==(BlockRange left, BlockRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlockRange left, BlockRange right)
        {
            return !(left == right);
        }

        public bool Equals(BlockRange other)
        {
            return From.Value.Equals(other.From.Value) &&
                To.Value.Equals(other.To.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is BlockRange other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}