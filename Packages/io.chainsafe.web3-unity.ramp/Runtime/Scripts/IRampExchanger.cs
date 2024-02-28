using System;
using System.Threading.Tasks;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    /// <summary>
    /// Interface for handling buying and selling crypto through a Ramp widget.
    /// </summary>
    public interface IRampExchanger
    {
        /// <summary>
        /// Event triggered when a new on-ramp purchase is initiated.
        /// This event signifies the initiation of an on-ramp purchase transaction after the user has completed the necessary information in the widget.
        /// </summary>
        /// <remarks>
        /// The event provides data about the initiated on-ramp purchase. Note that the transaction is not completed at this stage.
        /// </remarks>
        event Action<OnRampPurchaseData> OnRampPurchaseCreated;

        /// <summary>
        /// Event triggered when a new off-ramp sale is initiated.
        /// This event signifies the initiation of an off-ramp sale transaction after the user has completed the necessary information in the widget.
        /// </summary>
        /// <remarks>
        /// The event provides data about the initiated off-ramp sale. Note that the transaction is not completed at this stage.
        /// </remarks>
        event Action<OffRampSaleData> OffRampSaleCreated;

        /// <summary>
        /// Displays a widget to initiate the purchase of cryptocurrency.
        /// After the player fills out all required information in the widget, the transaction is initiated and this method returns transaction details.
        /// </summary>
        /// <param name="settings">Settings for the ramp buy widget.</param>
        /// <returns>Returns a task that completes with the on-ramp purchase data for the initiated transaction.</returns>
        Task<OnRampPurchaseData> BuyCrypto(RampBuyWidgetSettings settings);

        /// <summary>
        /// Displays a widget to initiate the sale of cryptocurrency.
        /// After the player fills out all required information in the widget, the transaction is initiated and this method returns transaction details.
        /// </summary>
        /// <param name="settings">Settings for the ramp sell widget.</param>
        /// <returns>Returns a task that completes with the off-ramp sale data for the initiated transaction.</returns>
        Task<OffRampSaleData> SellCrypto(RampSellWidgetSettings settings);

        /// <summary>
        /// Displays a widget to initiate either a purchase or sale of cryptocurrency based on the provided settings.
        /// After the player fills out all required information in the widget, the transaction is initiated and this method returns transaction details.
        /// </summary>
        /// <param name="settings">Combined settings for either buying or selling crypto.</param>
        /// <returns>Returns a task that completes with the transaction data for the initiated transaction.</returns>
        Task<RampTransactionData> BuyOrSellCrypto(RampBuyOrSellWidgetSettings settings);
    }
}