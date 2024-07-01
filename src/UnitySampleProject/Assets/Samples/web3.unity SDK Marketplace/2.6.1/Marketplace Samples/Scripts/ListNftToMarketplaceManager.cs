using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Model;
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
        [SerializeField] private TMP_Dropdown selectMarketplaceDropDown;
        private MarketplaceModel.ProjectMarketplacesResponse marketplaceList;

        #endregion

        #region Properties

        private string CollectionContractToListFrom { get; set; }
        private string MarketplaceContractToListTo { get; set; }
        private string TokenIdToList { get; set; }
        private string Price { get; set; }
        private string NftType { get; set; }
        private string BearerToken { get; set; }

        #endregion
        
        #region Methods
        
        /// <summary>
        /// Initializes objects.
        /// </summary>
        private void Start()
        {
            GetMarketplaceOptions();
        }

        /// <summary>
        /// Populates the marketplace drop down options.
        /// </summary>
        private async void GetMarketplaceOptions()
        {
            marketplaceList = await EvmMarketplace.GetProjectMarketplaces(BearerToken);
            if (marketplaceList.Marketplaces.Count <= 0) return;
            MarketplaceContractToListTo = marketplaceList.Marketplaces[0].Id;
            List<string> options = new List<string>();
            options.Add(marketplaceList.Marketplaces[0].Name);
            selectMarketplaceDropDown.ClearOptions();
            selectMarketplaceDropDown.AddOptions(options);
            selectMarketplaceDropDown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
        
        /// <summary>
        /// Sets selected marketplace when dropdown value is changed.
        /// </summary>
        /// <param name="index">The index of the selected option.</param>
        private void OnDropdownValueChanged(int index)
        {
            if (index < marketplaceList.Marketplaces.Count)
            {
                MarketplaceContractToListTo = marketplaceList.Marketplaces[index].Id;
            }
        }
        
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
            EventManagerMarketplace.ConfigureListNftToMarketplaceManager += OnConfigureListNftToMarketplaceManager;
            EventManagerMarketplace.ConfigureListNftToMarketplaceTxManager += IncomingMarketplaceListing;
            EventManagerMarketplace.CreateMarketplace += GetMarketplaceOptions;
            EventManagerMarketplace.ListNftToMarketplace += ListNftToMarketplace;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.ConfigureListNftToMarketplaceManager -= OnConfigureListNftToMarketplaceManager;
            EventManagerMarketplace.ConfigureListNftToMarketplaceTxManager -= IncomingMarketplaceListing;
            EventManagerMarketplace.CreateMarketplace -= GetMarketplaceOptions;
            EventManagerMarketplace.ListNftToMarketplace -= ListNftToMarketplace;
        }
        
        /// <summary>
        /// Configures properties for nft purchase.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="listNftToMarketplaceTxEventArgs">Args.</param>
        private void IncomingMarketplaceListing(object sender, EventManagerMarketplace.ListNftToMarketplaceTxEventArgs listNftToMarketplaceTxEventArgs)
        {
            CollectionContractToListFrom = listNftToMarketplaceTxEventArgs.CollectionContractToListFrom;
            MarketplaceContractToListTo = listNftToMarketplaceTxEventArgs.MarketplaceContractToListTo;
            TokenIdToList = listNftToMarketplaceTxEventArgs.TokenIdToList;
            Price = listNftToMarketplaceTxEventArgs.Price;
            NftType = listNftToMarketplaceTxEventArgs.NftType;
        }
        
        /// <summary>
        /// Configures tokens.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="listNftToMarketplaceConfigEventArgs">Args.</param>
        private void OnConfigureListNftToMarketplaceManager(object sender, EventManagerMarketplace.ListNftToMarketplaceConfigEventArgs listNftToMarketplaceConfigEventArgs)
        {
            BearerToken = listNftToMarketplaceConfigEventArgs.BearerToken;
        }
        
        #endregion
    }
}