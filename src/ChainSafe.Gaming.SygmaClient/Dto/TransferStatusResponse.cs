namespace ChainSafe.Gaming.SygmaClient.Dto
{
    public enum TransferStatus
    {
        Pending,
        Executed,
        Failed,
    }

    public class TransferStatusResponse
    {
        public TransferStatusResponse(TransferStatus status, string exploreUrl)
        {
            Status = status;
            ExploreUrl = exploreUrl;
        }

        public TransferStatus Status { get; }

        public string ExploreUrl { get; }
    }
}