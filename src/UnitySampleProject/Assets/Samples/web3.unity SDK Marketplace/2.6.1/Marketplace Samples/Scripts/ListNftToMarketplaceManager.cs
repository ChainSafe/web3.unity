using TMPro;
using UnityEngine;
using EvmMarketplace = Scripts.EVM.Marketplace.Marketplace;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages collection Nft minting.
    /// </summary>
    public class ListNftToMarketplaceManager : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private TMP_InputField priceInput;
        [SerializeField] private TMP_InputField amountInput;

        #endregion
        
        #region Methods
        
        /// <summary>
        /// List selected Nft to marketplace.
        /// </summary>
        private void ListNftToMarketplace()
        {
            //ListNftsToMarketplace();
        }
        
        /// <summary>
        /// Approves marketplace to list tokens
        /// </summary>
        public async void ApproveListNftsToMarketplace(string collectionContractToList, string marketplaceContractToListTo)
        {
            var response = await EvmMarketplace.SetApprovalMarketplace(collectionContractToList, marketplaceContractToListTo, "1155",true);
            Debug.Log($"TX: {response.TransactionHash}");
        }
    
        /// <summary>
        /// Revokes approval from marketplace to list tokens
        /// </summary>
        public async void RevokeApprovalListNftsToMarketplace(string collectionContractToList, string marketplaceContractToListTo)
        {
            var response = await EvmMarketplace.SetApprovalMarketplace(collectionContractToList, marketplaceContractToListTo, "1155",false);
            Debug.Log($"TX: {response.TransactionHash}");
        }
    
        /// <summary>
        /// Lists NFTs to the marketplace
        /// </summary>
        public async void ListNftsToMarketplace(string marketplaceContractToListTo,string collectionContractToList, string tokenIdToList, string weiPriceToList)
        {
            var response = await EvmMarketplace.ListNftsToMarketplace(marketplaceContractToListTo,collectionContractToList, tokenIdToList, weiPriceToList);
            Debug.Log($"TX: {response.TransactionHash}");
        }
        
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.ListNftToMarketplace += ListNftToMarketplace;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.ListNftToMarketplace -= ListNftToMarketplace;
        }
        
        #endregion
    }
}