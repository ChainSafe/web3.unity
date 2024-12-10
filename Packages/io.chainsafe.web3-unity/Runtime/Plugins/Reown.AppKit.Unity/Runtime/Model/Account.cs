using System;

namespace Reown.AppKit.Unity
{
    // https://chainagnostic.org/CAIPs/caip-10
    public readonly struct Account : IEquatable<Account>
    {
        public string Address { get; }
        public string ChainId { get; }

        public string AccountId
        {
            get => $"{ChainId}:{Address}";
        }

        public Account(string address, string chainId)
        {
            Address = address;
            ChainId = chainId;
        }

        public override string ToString()
        {
            return AccountId;
        }

        public bool Equals(Account other)
        {
            return AccountId == other.AccountId;
        }

        public override bool Equals(object obj)
        {
            return obj is Account other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Address != null ? Address.GetHashCode() : 0) * 397 ^
                       (ChainId != null ? ChainId.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Account left, Account right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Account left, Account right)
        {
            return !(left == right);
        }
    }
}