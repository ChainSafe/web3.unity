using System;
using ChainSafe.Gaming.Web3;
using TMPro;
using UnityEngine;
using EvmMarketplace = Scripts.EVM.Marketplace.Marketplace;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages marketplace creation.
    /// </summary>
    public class CreateMarketplaceManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_InputField descriptionInput;
        [SerializeField] private bool whiteListing;
        private bool processing;
        
        #endregion
        
        #region Properties
        
        private string BearerToken { get; set; }
    
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Uploads marketplace image.
        /// </summary>
        private void UploadMarketplaceImage()
        {
            if (processing) return;
            processing = true;
            CreateMarketplace(nameInput.text, descriptionInput.text, whiteListing);
        }
        
        /// <summary>
        /// Creates a marketplace.
        /// </summary>
        public async void CreateMarketplace(string marketplaceName, string marketplaceDescription, bool marketplaceWhiteListing)
        {
            try
            {
                var response = await EvmMarketplace.CreateMarketplace(BearerToken, marketplaceName, marketplaceDescription, marketplaceWhiteListing);
                Debug.Log($"TX: {response.TransactionHash}");
                EventManagerMarketplace.RaiseCreateMarketplace();
                processing = false;
            }
            catch (Web3Exception e)
            {
                processing = false;
                Debug.Log($"Creation failed: {e}");
            }
            catch (Exception)
            {
                processing = false;
            }
        }
    
        /// <summary>
        /// Deletes a marketplace that isn't on chain yet
        /// </summary>
        public async void DeleteMarketplace(string marketplaceToDelete)
        {
            var response = await EvmMarketplace.DeleteMarketplace(BearerToken,marketplaceToDelete);
            Debug.Log(response);
        }
        
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.UploadMarketplaceImage += UploadMarketplaceImage;
            EventManagerMarketplace.ConfigureMarketplaceCreateManager += OnConfigureMarketPlaceCreateManager;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.UploadMarketplaceImage -= UploadMarketplaceImage;
            EventManagerMarketplace.ConfigureMarketplaceCreateManager -= OnConfigureMarketPlaceCreateManager;
        }
        
        /// <summary>
        /// Configures class properties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnConfigureMarketPlaceCreateManager(object sender, EventManagerMarketplace.MarketplaceCreateConfigEventArgs args)
        {
            BearerToken = args.BearerToken;
        }
        
        #endregion
    }
}
