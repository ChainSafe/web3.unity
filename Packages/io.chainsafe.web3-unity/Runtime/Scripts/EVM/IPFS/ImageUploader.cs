using Scripts.EVM.Remote;
using UnityEngine;

namespace ChainSafe.Gaming
{
    public class ImageUploader : MonoBehaviour
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>
        /// Emits an event with image data for uploading with webgl
        /// </summary>
        /// <param name="imageData">Image data</param>
        public void OnImageSelected(string imageData)
        {
            CSServer.OnImageSelected(imageData);
        }
        #endif
    }
}