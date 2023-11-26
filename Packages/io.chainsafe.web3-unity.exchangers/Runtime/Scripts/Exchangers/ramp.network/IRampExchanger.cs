using System.Threading.Tasks;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public interface IRampExchanger
    {
        Task<OnRampPurchaseData> BuyCrypto(RampBuyWidgetSettings settings);
        Task<OffRampSaleData> SellCrypto(RampSellWidgetSettings settings);
        Task<RampTransactionData> BuyOrSellCrypto(RampBuyOrSellWidgetSettings settings);
    }
}