using System;

namespace ChainSafe.Gaming.RPC.Events
{
    public struct EventDictionaryKey : IEquatable<EventDictionaryKey>
    {
        public Type EventType { get; set; }

        public string[] ContractAddresses { get; set; }

        public static bool operator ==(EventDictionaryKey left, EventDictionaryKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EventDictionaryKey left, EventDictionaryKey right)
        {
            return !(left == right);
        }

        public bool Equals(EventDictionaryKey other)
        {
            // Compare types
            if (EventType != other.EventType)
            {
                return false;
            }

            // Compare string arrays for contract addresses
            if (ContractAddresses.Length != other.ContractAddresses.Length)
            {
                return false;
            }

            for (int i = 0; i < ContractAddresses.Length; i++)
            {
                if (ContractAddresses[i] != other.ContractAddresses[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is EventDictionaryKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            // Get hash codes for both Type and array content
            int hash = EventType.GetHashCode();
            foreach (var address in ContractAddresses)
            {
                hash = (hash * 31) + (address != null ? address.GetHashCode() : 0);
            }

            return hash;
        }
    }
}