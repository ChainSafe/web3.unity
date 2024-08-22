using System;
using ChainSafe.Gaming.Web3;
using TMPro;
using UnityEngine;
using EvmMarketplace = Scripts.EVM.Marketplace.Marketplace;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages the collection creation.
    /// </summary>
    public class CreateCollectionManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Dropdown typeDropDown;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_InputField descriptionInput;
        [SerializeField] private bool publicMinting;
        private bool processing;

        #endregion
        
        #region Properties
        
        private string BearerToken { get; set; }
    
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Uploads collection image.
        /// </summary>
        private void UploadCollectionImage()
        {
            if (processing) return;
            processing = true;
            switch (typeDropDown.options[typeDropDown.value].text)
            {
                case "721":
                    Create721Collection(nameInput.text, descriptionInput.text, publicMinting);
                    break;
                case "1155":
                    Create1155Collection(nameInput.text, descriptionInput.text, publicMinting);
                    break;
            }
        }
        
        /// <summary>
        /// Creates a 721 collection.
        /// </summary>
        public async void Create721Collection(string collectionName721, string collectionDescription721, bool collectionMintingPublic721)
        {
            try
            {
                var response = await EvmMarketplace.Create721Collection(BearerToken, collectionName721, collectionDescription721, collectionMintingPublic721);
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
                        Debug.Log($"Creation failed: {e}");
                        break;
                }
            }
        }
        
        /// <summary>
        /// Creates a 1155 collection.
        /// </summary>
        public async void Create1155Collection(string collectionName1155, string collectionDescription1155, bool collectionMintingPublic1155)
        {
            try
            {
                var response = await EvmMarketplace.Create1155Collection(BearerToken, collectionName1155, collectionDescription1155, collectionMintingPublic1155);
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
                        Debug.Log($"Creation failed: {e}");
                        break;
                }
            }
        }
        
        
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.UploadCollectionImage += UploadCollectionImage;
            EventManagerMarketplace.ConfigureCollectionCreateManager += OnConfigureCollectionCreateManager;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.UploadCollectionImage -= UploadCollectionImage;
            EventManagerMarketplace.ConfigureCollectionCreateManager -= OnConfigureCollectionCreateManager;
        }
        
        /// <summary>
        /// Configures class properties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnConfigureCollectionCreateManager(object sender, EventManagerMarketplace.CollectionCreateConfigEventArgs args)
        {
            BearerToken = args.BearerToken;
        }
        
        #endregion
    }
}