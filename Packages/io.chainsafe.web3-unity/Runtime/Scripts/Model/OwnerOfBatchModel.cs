namespace ChainSafe.Gaming.UnityPackage.Model
{
    public struct OwnerOfBatchModel
    {
        public string Owner { get; set; }
        public string TokenId { get; set; }

        public override string ToString()
        {
            return $"{nameof(Owner)}: {Owner}, {nameof(TokenId)}: {TokenId}";
        }
    }
}