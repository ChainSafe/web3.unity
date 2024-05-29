namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    public struct OwnerOfBatchModel
    {
        public bool Failure { get; set; }

        public string Owner { get; set; }

        public string TokenId { get; set; }

        public override string ToString()
        {
            return $"{nameof(Owner)}: {Owner}, {nameof(TokenId)}: {TokenId}";
        }
    }
}