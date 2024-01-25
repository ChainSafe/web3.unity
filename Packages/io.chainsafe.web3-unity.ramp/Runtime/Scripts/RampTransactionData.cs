namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public class RampTransactionData
    {
        public OnRampPurchaseData? PurchaseData { get; set; }
        public OffRampSaleData? SaleData { get; set; }

        public bool IsPurchase => PurchaseData != null;
        public bool IsSale => SaleData != null;

        public override string ToString()
        {
            if (IsPurchase)
            {
                return $"On-Ramp: ({PurchaseData!.Value.Status}): {PurchaseData.ToString()}";
            }
            else if (IsSale)
            {
                return $"Off-Ramp: {SaleData}";
            }

            return "Error :(";
        }
    }
}