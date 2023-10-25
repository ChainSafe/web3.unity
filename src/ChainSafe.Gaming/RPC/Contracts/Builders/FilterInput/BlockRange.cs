using System;
using System.Numerics;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Evm.Contracts.Builders.FilterInput
{
    /// <summary>
    /// Provides functionality to build and manage Ethereum contracts based on the ABI and address.
    /// </summary>
    public readonly struct BlockRange :
        IEquatable<BlockRange>
    {
        private readonly int hashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockRange"/> struct.
        /// </summary>
        /// <param name="from">Starting block number.</param>
        /// <param name="to">Ending block number.</param>
        public BlockRange(ulong from, ulong to)
            : this(new BigInteger(from), new BigInteger(to))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockRange"/> struct.
        /// </summary>
        /// <param name="from">Starting block represented as <see cref="BigInteger"/>.</param>
        /// <param name="to">Ending block represented as <see cref="BigInteger"/>.</param>
        public BlockRange(BigInteger from, BigInteger to)
            : this(new HexBigInteger(from), new HexBigInteger(to))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockRange"/> struct.
        /// </summary>
        /// <param name="from">Starting block represented as <see cref="HexBigInteger"/>.</param>
        /// <param name="to">Ending block represented as <see cref="HexBigInteger"/>.</param>
        public BlockRange(HexBigInteger from, HexBigInteger to)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            BlockCount = (To.Value - From.Value) + 1;
            hashCode = new { From, To }.GetHashCode();
        }

        /// <summary>
        /// Gets the starting block.
        /// </summary>
        public HexBigInteger From { get; }

        /// <summary>
        /// Gets the ending block.
        /// </summary>
        public HexBigInteger To { get; }

        /// <summary>
        /// Gets the total number of blocks represented in the range.
        /// </summary>
        public BigInteger BlockCount { get; }

        /// <summary>
        /// Equality operator.
        /// </summary>
        public static bool operator ==(BlockRange left, BlockRange right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        public static bool operator !=(BlockRange left, BlockRange right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Checks equality with other <see cref="BlockRange"/>.
        /// </summary>
        /// <returns>Return true if equal.</returns>
        public bool Equals(BlockRange other)
        {
            return From.Value.Equals(other.From.Value) && To.Value.Equals(other.To.Value);
        }

        /// <summary>
        /// Checks equality with given object.
        /// </summary>
        /// <returns>Return true if equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is BlockRange other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <summary>
        /// Gets the struct hash code.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}
