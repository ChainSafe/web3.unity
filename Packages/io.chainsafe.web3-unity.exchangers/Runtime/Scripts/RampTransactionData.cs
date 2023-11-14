using JetBrains.Annotations;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public class RampTransactionData
    {
        public OnRampPurchaseData? PurchaseData { get; set; }
        [CanBeNull] public OffRampSaleData SellData { get; set; }

        public bool Purchased => PurchaseData != null;
        public bool Sold => SellData != null;

        public override string ToString()
        {
            if (Purchased)
            {
                return $"Successful purchase! {PurchaseData.ToString()}";
            }
            else if (Sold)
            {
                return $"Successful sale! {SellData}";
            }

            return "Error :(";
        }
    }
}