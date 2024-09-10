using System;
using ChainSafe.Gaming.Web3;
using TMPro;
using UnityEngine;
using EvmMarketplace = Scripts.EVM.Marketplace.Marketplace;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages collection Nft minting.
    /// </summary>
    public class MintCollectionNftManager : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_InputField descriptionInput;
        [SerializeField] private TMP_InputField amountInput;
        private bool processing;

        #endregion
        
        #region Properties

        private string CollectionContractToListFrom { get; set; }
        private string CollectionTypeToListFrom { get; set; }
        private string BearerToken { get; set; }

        #endregion
        
        #region Methods
        
        /// <summary>
        /// Uploads collection image.
        /// </summary>
        private void UploadNftImage()
        {
            if (processing) return;
            processing = true;
            if (amountInput.text == "")
            {
                amountInput.text = "1";
            }
            Debug.Log($"TYPE: {CollectionTypeToListFrom}");
            switch (CollectionTypeToListFrom)
            {
                case "ERC721":
                    Mint721CollectionNft(BearerToken, CollectionContractToListFrom, nameInput.text, descriptionInput.text);
                    break;
                case "ERC1155":
                    Mint1155CollectionNft(BearerToken, CollectionContractToListFrom, nameInput.text, amountInput.text, descriptionInput.text);
                    break;
                default:
                    throw new Exception($"Type not valid: {CollectionTypeToListFrom}");
            }
        }

        /// <summary>
        /// Mints an NFT to a 721 collection
        /// </summary>
        public async void Mint721CollectionNft(string bearerToken, string collectionContract721, string name721, string description721)
        {
            try
            {
                var response = await EvmMarketplace.Mint721CollectionNft(bearerToken, collectionContract721, name721, description721);
                Debug.Log($"TX: {response.TransactionHash}");
                processing = false;
            }
            catch (Exception e)
            {
                processing = false;
                switch (e)
                {
                    case Web3Exception web3Ex:
                        Debug.Log($"Web3 exception: {web3Ex}");
                        break;
                    
                    default:
                        Debug.Log($"Minting failed: {e}");
                        break;
                }
            }
        }

        /// <summary>
        /// Mints an NFT to a 1155 collection
        /// </summary>
        public async void Mint1155CollectionNft(string bearerToken, string collectionContract1155, string name1155, string amount1155, string description1155)
        {
            try
            {
                var response = await EvmMarketplace.Mint1155CollectionNft(bearerToken, collectionContract1155, amount1155, name1155, description1155);
                Debug.Log($"TX: {response.TransactionHash}");
                processing = false;
            }
            catch (Exception e)
            {
                processing = false;
                switch (e)
                {
                    case Web3Exception web3Ex:
                        Debug.Log($"Web3 exception: {web3Ex}");
                        break;
                    
                    default:
                        Debug.Log($"Minting failed: {e}");
                        break;
                }
            }
        }
        
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.MintNftToCollection += UploadNftImage;
            EventManagerMarketplace.ConfigureMintCollectionNftManager += OnConfigureMintCollectionNftManager;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.MintNftToCollection -= UploadNftImage;
            EventManagerMarketplace.ConfigureMintCollectionNftManager -= OnConfigureMintCollectionNftManager;
        }
        
        /// <summary>
        /// Configures tokens.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="mintCollectionNftConfigEventArgs">Args.</param>
        private void OnConfigureMintCollectionNftManager(object sender, EventManagerMarketplace.MintCollectionNftConfigEventArgs mintCollectionNftConfigEventArgs)
        {
            if (mintCollectionNftConfigEventArgs.BearerToken != null)
            {
                BearerToken = mintCollectionNftConfigEventArgs.BearerToken;
            }
            CollectionContractToListFrom = mintCollectionNftConfigEventArgs.CollectionContractToListFrom;
            CollectionTypeToListFrom = mintCollectionNftConfigEventArgs.CollectionTypeToListFrom;
            switch (mintCollectionNftConfigEventArgs.CollectionTypeToListFrom)
            {
                case "ERC721":
                    Debug.Log("DISABLING AMOUNT INPUT");
                    amountInput.gameObject.SetActive(false);
                    break;
                case "ERC1155":
                    Debug.Log("ENABLING AMOUNT INPUT");
                    amountInput.gameObject.SetActive(true);
                    break;
            }
        }
        
        #endregion
    }
}