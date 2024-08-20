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
        
        [SerializeField] private TMP_Dropdown typeDropDown;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_InputField descriptionInput;
        [SerializeField] private TMP_InputField amountInput;
        private bool processing;

        #endregion
        
        #region Properties

        private string CollectionContractToListFrom { get; set; }
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
            nameInput.text ??= " ";
            descriptionInput.text ??= " ";
            switch (typeDropDown.options[typeDropDown.value].text)
            {
                case "721":
                    Mint721CollectionNft(BearerToken, CollectionContractToListFrom, nameInput.text, descriptionInput.text);
                    break;
                case "1155":
                    Mint1155CollectionNft(BearerToken, CollectionContractToListFrom, nameInput.text, amountInput.text, descriptionInput.text);
                    break;
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
            catch (Web3Exception e)
            {
                processing = false;
                Debug.Log($"Minting failed: {e}");
            }
            catch (Exception)
            {
                processing = false;
            }
        }
        
        /// <summary>
        /// Mints an NFT to a 1155 collection
        /// </summary>
        public async void Mint1155CollectionNft(string bearerToken, string collectionContract1155, string name1155, string amount1155, string description1155)
        {
            try
            {
                var response = await EvmMarketplace.Mint1155CollectionNft(bearerToken, collectionContract1155, name1155, amount1155, description1155);
                Debug.Log($"TX: {response.TransactionHash}");
                processing = false;
            }
            catch (Web3Exception e)
            {
                processing = false;
                Debug.Log($"Minting failed: {e}");
            }
            catch (Exception)
            {
                processing = false;
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
        }
        
        #endregion
    }
}