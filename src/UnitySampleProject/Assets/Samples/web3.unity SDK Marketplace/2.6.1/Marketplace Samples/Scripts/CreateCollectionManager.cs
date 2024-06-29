using UnityEngine;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages the collection creation.
    /// </summary>
    public class CreateCollectionManager : MonoBehaviour
    {
        #region Methods
        
        /// <summary>
        /// Uploads collection image.
        /// </summary>
        private void UploadCollectionImage()
        {
            Debug.Log("TODO: Uploading image");
        }
        
        /// <summary>
        /// Toggles marketplace menu.
        /// </summary>
        private void ToggleCreateCollectionMenu()
        {
            Debug.Log("TODO: Toggle logic");
        }
        
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            EventManagerMarketplace.UploadCollectionImage += UploadCollectionImage;
            EventManagerMarketplace.ToggleCreateCollectionMenu += ToggleCreateCollectionMenu;
        }
        
        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            EventManagerMarketplace.UploadCollectionImage -= UploadCollectionImage;
            EventManagerMarketplace.ToggleCreateCollectionMenu -= ToggleCreateCollectionMenu;
        }
        
        #endregion
    }
}