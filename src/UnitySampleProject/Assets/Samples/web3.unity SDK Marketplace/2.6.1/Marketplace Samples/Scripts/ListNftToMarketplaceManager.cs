using System.Threading.Tasks;
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

        #region Properties

        private string CollectionContractToListFrom { get; set; }
        private string MarketplaceContractToListTo { get; set; }
        private string TokenIdToList { get; set; }
        private string Price { get; set; }
        private string NftType { get; set; }

        #endregion
        
        #region Methods
        
        /// <summary>
        /// List selected Nft to marketplace.
        /// </summary>
        private async void ListNftToMarketplace()
        {
            await ApproveListNftsToMarketplace(CollectionContractToListFrom, MarketplaceContractToListTo, NftType);
            await ListNftsToMarketplace(MarketplaceContractToListTo, CollectionContractToListFrom, TokenIdToList,  priceInput.text);
        }
        
        /// <summary>
        /// Approves marketplace to list tokens
        /// </summary>
        public async Task ApproveListNftsToMarketplace(string collectionContractToList, string marketplaceContractToListTo, string nftType)
        {
            var response = await EvmMarketplace.SetApprovalMarketplace(collectionContractToList, marketplaceContractToListTo, nftType,true);
            Debug.Log($"TX: {response.TransactionHash}");
        }
    
        /// <summary>
        /// Revokes approval from marketplace to list tokens
        /// </summary>
        public async Task RevokeApprovalListNftsToMarketplace(string collectionContractToList, string marketplaceContractToListTo, string nftType)
        {
            var response = await EvmMarketplace.SetApprovalMarketplace(collectionContractToList, marketplaceContractToListTo, nftType,false);
            Debug.Log($"TX: {response.TransactionHash}");
        }
    
        /// <summary>
        /// Lists NFTs to the marketplace
        /// </summary>
        public async Task ListNftsToMarketplace(string marketplaceContractToListTo,string collectionContractToList, string tokenIdToList, string weiPriceToList)
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
            EventManagerMarketplace.ConfigureListNftToMarketplaceManager += IncomingMarketplaceListing;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.ListNftToMarketplace -= ListNftToMarketplace;
            EventManagerMarketplace.ConfigureListNftToMarketplaceManager -= IncomingMarketplaceListing;
        }
        
        //TODO CollectionContractToListFrom needs to be populated.
        /// <summary>
        /// Configures properties for nft purchase.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="listNftToMarketplaceConfigEventArgs">Args.</param>
        private void IncomingMarketplaceListing(object sender, EventManagerMarketplace.ListNftToMarketplaceConfigEventArgs listNftToMarketplaceConfigEventArgs)
        {
            CollectionContractToListFrom = listNftToMarketplaceConfigEventArgs.CollectionContractToListFrom;
            MarketplaceContractToListTo = listNftToMarketplaceConfigEventArgs.MarketplaceContractToListTo;
            TokenIdToList = listNftToMarketplaceConfigEventArgs.TokenIdToList;
            Price = listNftToMarketplaceConfigEventArgs.Price;
            NftType = listNftToMarketplaceConfigEventArgs.NftType;
        }
        
        #endregion
    }
}