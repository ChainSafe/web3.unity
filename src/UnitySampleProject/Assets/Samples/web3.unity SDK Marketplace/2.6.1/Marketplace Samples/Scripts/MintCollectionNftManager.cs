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
                    Mint721CollectionNft(nameInput.text, descriptionInput.text);
                    break;
                case "1155":
                    Mint1155CollectionNft(nameInput.text, descriptionInput.text, amountInput.text);
                    break;
            }
        }
        
        /// <summary>
        /// Mints an NFT to a 721 collection
        /// </summary>
        public async void Mint721CollectionNft(string collectionContract721, string uri721)
        {
            try
            {
                var response = await EvmMarketplace.Mint721CollectionNft(collectionContract721, uri721);
                Debug.Log($"TX: {response.TransactionHash}");
            }
            catch (Web3Exception e)
            {
                processing = false;
                Debug.Log($"Minting failed: {e}");
            }
        }
        
        /// <summary>
        /// Mints an NFT to a 1155 collection
        /// </summary>
        public async void Mint1155CollectionNft(string collectionContract1155, string uri1155, string amount1155)
        {
            try
            {
                var response = await EvmMarketplace.Mint1155CollectionNft(collectionContract1155, uri1155, amount1155);
                Debug.Log($"TX: {response.TransactionHash}");
            }
            catch (Web3Exception e)
            {
                processing = false;
                Debug.Log($"Minting failed: {e}");
            }
        }
        
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.UploadNftImageToCollection += UploadNftImage;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.UploadNftImageToCollection -= UploadNftImage;
        }
        
        #endregion
    }
}