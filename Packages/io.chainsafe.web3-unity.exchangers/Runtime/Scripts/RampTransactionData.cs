using JetBrains.Annotations;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public class RampTransactionData
    {
        public OnRampPurchaseData? PurchaseData { get; set; }
        [CanBeNull] public OffRampSaleData SellData { get; set; }

        public bool Purchased => PurchaseData != null;
        public bool Sold => SellData != null;
    }
}