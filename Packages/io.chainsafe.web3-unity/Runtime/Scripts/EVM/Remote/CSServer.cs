using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.EVM.Remote
{
    public class CSServer
    {
        #region Fields
        
        [DllImport("__Internal")]
        private static extern void UploadImage();
        public static event EventHandler<byte[]> ImageSelected;
        private static readonly string host = "https://api.gaming.chainsafe.io/v1/projects/";

        #endregion

        #region Methods

        /// <summary>
        /// Unity web request helper function to retrieve data.
        /// </summary>
        /// <param name="_path">The path suffix to call</param>
        /// <returns>Server response</returns>
        public static async Task<T> GetData<T>(string _path)
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get($"{host}{Web3Accessor.Web3.ProjectConfig.ProjectId}{_path}");
            await webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: Your project ID doesn't have a marketplace or the token ID doesn't exist, please go to dashboard and create items " + webRequest.error);
                return default;
            }
            var json = webRequest.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<T>(json);
            return response;
        }
        
        /// <summary>
        /// Unity web request helper function to delete data entries from collections/marketplace.
        /// </summary>
        /// <param name="_bearerToken">Bearer token to access dashboard services</param>
        /// <param name="_path">The path suffix to call</param>
        /// <returns>Server response</returns>
        public static async Task<string> DeleteData(string _bearerToken, string _path)
        {
            using UnityWebRequest request = UnityWebRequest.Delete($"{host}{Web3Accessor.Web3.ProjectConfig.ProjectId}{_path}");
            request.SetRequestHeader("Authorization", $"Bearer {_bearerToken}");
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error deleting: {request.error}");
                return request.error;
            }
            return "Deleted successfully";
        }
        
        /// <summary>
        /// Unity web request helper function to create data entries with collections/marketplace.
        /// </summary>
        /// <param name="_bearerToken">Bearer token to access dashboard services</param>
        /// <param name="_path">The path suffix to call</param>
        /// <param name="_formData">Form data used in the call</param>
        /// <returns>Server response</returns>
        public static async Task<string> CreateData(string _bearerToken, string _path,
            List<IMultipartFormSection> _formData)
        {
            using (UnityWebRequest request = UnityWebRequest.Post($"{host}{Web3Accessor.Web3.ProjectConfig.ProjectId}{_path}", _formData))
            {
                request.SetRequestHeader("Authorization", $"Bearer {_bearerToken}");
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Creation failed: " + request.downloadHandler.text);
                    return request.error;
                }
                return request.downloadHandler.text;
            }
        }

        #region Utilities
        
        /// <summary>
        /// Gets the binary data of a png image with a compiler conditional for different platforms.
        /// </summary>
        /// <returns>Byte array of image data</returns>
        public static async Task<byte[]> GetImageData()
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            var imageData = await UploadImageWebGL();
            return imageData;
            #elif UNITY_EDITOR
            var imageData = await UploadImageEditor();
            return imageData;
            #elif UNITY_STANDALONE_WIN
            var imageData = await UploadImageWindows();
            return imageData;
            #elif UNITY_STANDALONE_OSX
            var imageData = await UploadImageOsx();
            return imageData;
            #else
            Debug.LogError("File picking is not implemented for this platform.");
            return null;
            #endif
        }
        
        #if UNITY_EDITOR
        public static async Task<byte[]> UploadImageEditor()
        {
            var imagePath = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg,gif");
            while (string.IsNullOrEmpty(imagePath)) return null;
            UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + imagePath);
            await www.SendWebRequest();
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            var imageData = texture.EncodeToPNG();
            return imageData;
        }
        #endif
        
        #if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>
        /// Uploads an image in webgl builds
        /// </summary>
        /// <returns>Image data</returns>
        public static async Task<byte[]> UploadImageWebGL()
        {
            var imageDataTask = new TaskCompletionSource<byte[]>();
            // Event handler to set the result when the image is selected
            void OnImageSelectedHandler(object sender, byte[] imageData)
            {
                imageDataTask.SetResult(imageData);
                // Unsubscribe from the event after handling it
                ImageSelected -= OnImageSelectedHandler;
            }
            ImageSelected += OnImageSelectedHandler;
            UploadImage();
            var imageData = await imageDataTask.Task;
            return imageData;
        }
        
        /// <summary>
        /// Invokes event to pass image data from js function
        /// </summary>
        /// <param name="base64Data">Image data</param>
        public static void OnImageSelected(string imageData)
        {
            try
            {
                // Remove metadata from url
                var base64String = imageData.Substring(imageData.IndexOf(",") + 1);
                // Convert data URL to byte array
                byte[] imageDataBytes = Convert.FromBase64String(base64String);
                // Invoke event to complete the upload tasks
                ImageSelected?.Invoke(null, imageDataBytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endif
        
        #if UNITY_STANDALONE_WIN
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetActiveWindow();

        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class OpenFileName
        {
            public int structSize = Marshal.SizeOf(typeof(OpenFileName));
            public IntPtr dlgOwner = IntPtr.Zero;
            public IntPtr instance = IntPtr.Zero;

            public string filter = null;
            public string customFilter = null;
            public int maxCustFilter = 0;
            public int filterIndex = 0;

            public string file = null;
            public int maxFile = 0;

            public string fileTitle = null;
            public int maxFileTitle = 0;

            public string initialDir = null;

            public string title = null;

            public int flags = 0;
            public short fileOffset = 0;
            public short fileExtension = 0;

            public string defExt = null;

            public IntPtr custData = IntPtr.Zero;
            public IntPtr hook = IntPtr.Zero;

            public string templateName = null;

            public IntPtr reservedPtr = IntPtr.Zero;
            public int reservedInt = 0;
            public int flagsEx = 0;
        }

        private const int OFN_EXPLORER = 0x00080000;
        private const int OFN_FILEMUSTEXIST = 0x00001000;
        private const int OFN_PATHMUSTEXIST = 0x00000800;
        
        public static async Task<byte[]> UploadImageWindows()
        {
            string imagePath = OpenFilePanel("Select Image", "", "Image files\0*.png;*.jpg;*.jpeg;*.gif\0\0");
            if (string.IsNullOrEmpty(imagePath)) return null;
            UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + imagePath);
            await www.SendWebRequest();
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            var imageData = texture.EncodeToPNG();
            return imageData;
        }

        private static string OpenFilePanel(string title, string directory, string filter)
        {
            OpenFileName ofn = new OpenFileName();
            ofn.structSize = Marshal.SizeOf(typeof(OpenFileName));
            ofn.filter = filter.Replace('|', '\0') + '\0';
            ofn.file = new string(new char[256]);
            ofn.maxFile = ofn.file.Length;
            ofn.fileTitle = new string(new char[64]);
            ofn.maxFileTitle = ofn.fileTitle.Length;
            ofn.initialDir = directory.Replace('/', '\\');
            ofn.title = title;
            ofn.flags = OFN_EXPLORER | OFN_FILEMUSTEXIST | OFN_PATHMUSTEXIST;
            ofn.dlgOwner = GetActiveWindow();

            if (GetOpenFileName(ofn))
            {
                return ofn.file;
            }

            return null;
        }
        #endif

        #if UNITY_STANDALONE_OSX
        [DllImport("__Internal")]
        private static extern void ShowOpenFileDialog(string title, string[] allowedFileTypes, int allowedFileTypesCount, IntPtr callback);

        private TaskCompletionSource<string> tcs;
        
        public async Task<byte[]> UploadImageOsx()
        {
            string imagePath = await OpenFilePanelMac("Select Image", "png,jpg,jpeg,gif");
            if (string.IsNullOrEmpty(imagePath)) return null;
            UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + imagePath);
            await www.SendWebRequest();
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            var imageData = texture.EncodeToPNG();
            return imageData;
        }        

        private Task<string> OpenFilePanelMac(string title, params string[] allowedFileTypes)
        {
            tcs = new TaskCompletionSource<string>();
            ShowOpenFileDialog(title, allowedFileTypes, allowedFileTypes.Length, Marshal.GetFunctionPointerForDelegate((Action<string>)OnFileSelectedMac));
            return tcs.Task;
        }

        private void OnFileSelectedMac(string path)
        {
            tcs.SetResult(path);
        }
        #endif

        #endregion

        #endregion

    }
}