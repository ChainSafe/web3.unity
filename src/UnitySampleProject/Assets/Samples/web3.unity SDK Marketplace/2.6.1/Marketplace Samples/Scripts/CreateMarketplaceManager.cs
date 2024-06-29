using UnityEngine;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages marketplace creation.
    /// </summary>
    public class CreateMarketplaceManager : MonoBehaviour
    {
        #region Methods
        
        /// <summary>
        /// Uploads marketplace image.
        /// </summary>
        private void UploadMarketplaceImage()
        {
            Debug.Log("TODO: Uploading image");
        }

        /// <summary>
        /// Toggles marketplace menu.
        /// </summary>
        private void ToggleCreateMarketplaceMenu()
        {
            Debug.Log("TODO: Toggle logic");
        }
        
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.UploadMarketplaceImage += UploadMarketplaceImage;
            EventManagerMarketplace.ToggleCreateMarketplaceMenu += ToggleCreateMarketplaceMenu;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.UploadMarketplaceImage -= UploadMarketplaceImage;
            EventManagerMarketplace.ToggleCreateMarketplaceMenu -= ToggleCreateMarketplaceMenu;
        }
        
        #endregion
    }
}
