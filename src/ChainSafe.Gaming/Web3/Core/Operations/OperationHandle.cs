namespace ChainSafe.Gaming.Web3.Core.Operations
{
    public struct OperationHandle
    {
        private int id;

        public static bool operator ==(OperationHandle left, OperationHandle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OperationHandle left, OperationHandle right)
        {
            return !left.Equals(right);
        }

        public static OperationHandle Next(OperationHandle previous)
        {
            return new OperationHandle { id = previous.id + 1 };
        }

        public bool Equals(OperationHandle other)
        {
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            return obj is OperationHandle other && Equals(other);
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}