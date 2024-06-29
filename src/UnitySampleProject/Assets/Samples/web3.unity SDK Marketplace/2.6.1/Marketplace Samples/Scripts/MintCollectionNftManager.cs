using UnityEngine;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages collection Nft minting.
    /// </summary>
    public class MintCollectionNftManager : MonoBehaviour
    {
        #region Methods
        
        /// <summary>
        /// Uploads collection image.
        /// </summary>
        private void UploadNftImage()
        {
            Debug.Log("TODO: Uploading image");
        }
        
        /// <summary>
        /// Toggles marketplace menu.
        /// </summary>
        private void ToggleMintNftToCollectionMenu()
        {
            Debug.Log("TODO: Toggle logic");
        }
        
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.UploadNftToCollectionImage += UploadNftImage;
            EventManagerMarketplace.ToggleMintNftToCollectionMenu += ToggleMintNftToCollectionMenu;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.UploadNftToCollectionImage -= UploadNftImage;
            EventManagerMarketplace.ToggleMintNftToCollectionMenu -= ToggleMintNftToCollectionMenu;
        }
        
        #endregion
    }
}