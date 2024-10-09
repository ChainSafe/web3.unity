using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using UnityEngine;
using UnityEngine.Networking;

public class UploadPlatforms
{

    #region Fields

    public static event EventHandler<byte[]> ImageSelected;

    #endregion

    #region Methods

    /// <summary>
    /// Gets the binary data of a png image with a compiler conditional for different platforms.
    /// </summary>
    /// <returns>Byte array of image data</returns>
    public static async Task<byte[]> GetImageData()
    {
        byte[] imageData = null;
#if UNITY_WEBGL && !UNITY_EDITOR
        imageData = await UploadImageWebGL();
        return imageData;
#elif UNITY_EDITOR
        imageData = await UploadImageEditor();
        return imageData;
#elif UNITY_STANDALONE_WIN
        imageData = await UploadImageWindows();
        return imageData;
#elif UNITY_STANDALONE_OSX
        imageData = await UploadImageOsx();
        return imageData;
#elif UNITY_IOS
        imageData = await UploadImageIOS();
        return imageData;
#else
        Debug.LogError("File picking is not implemented for this platform.");
        return null;
#endif
    }

#if UNITY_EDITOR
    private static async Task<byte[]> UploadImageEditor()
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
    
    [DllImport("__Internal")]
    private static extern void UploadImage();
    
    /// <summary>
    /// Uploads an image in webgl builds
    /// </summary>
    /// <returns>Image data</returns>
    private static async Task<byte[]> UploadImageWebGL()
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
    public static void OnImageSelected(string _imageData)
    {
        try
        {
            // Remove metadata from url
            var base64String = _imageData.Substring(_imageData.IndexOf(",") + 1);
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
    
    private static async Task<byte[]> UploadImageWindows()
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
    [DllImport("MacOSFilePicker", EntryPoint = "showOpenFileDialogWithTitle")]
    private static extern void showOpenFileDialog(string title, string[] allowedFileTypes, int allowedFileTypesCount, IntPtr callback);

    private static TaskCompletionSource<string> tcs;
    
    private static async Task<byte[]> UploadImageOsx()
    {
        string imagePath = await OpenFilePanelMac("Select Image", "public.png", "public.jpg", "public.jpeg", "public.gif");
        if (string.IsNullOrEmpty(imagePath)) return null;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + imagePath);
        await www.SendWebRequest();
        Texture2D texture = DownloadHandlerTexture.GetContent(www);
        var imageData = texture.EncodeToPNG();
        return imageData;
    }        

    private static Task<string> OpenFilePanelMac(string title, params string[] allowedFileTypes)
    {
        tcs = new TaskCompletionSource<string>();
        showOpenFileDialog(title, allowedFileTypes, allowedFileTypes.Length, Marshal.GetFunctionPointerForDelegate((Action<string>)OnFileSelectedMac));
        return tcs.Task;
    }

    private static void OnFileSelectedMac(string path)
    {
        tcs.SetResult(path);
    }
#endif

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void showActionSheet(string title, string[] allowedFileTypes, int allowedFileTypesCount, IntPtr callback);

    private static TaskCompletionSource<string> tcs;
    

    public static async Task<byte[]> UploadImageIOS()
    {
        string imagePath = await OpenFilePaneliOS("Select Source", "public.png", "public.jpg", "public.jpeg", "public.gif");
        Debug.Log(imagePath);
        if (string.IsNullOrEmpty(imagePath)) return null;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + imagePath);
        await www.SendWebRequest();
        Texture2D texture = DownloadHandlerTexture.GetContent(www);
        var imageData = texture.EncodeToPNG();
        return imageData;
    }

    private static Task<string> OpenFilePaneliOS(string title, params string[] allowedFileTypes)
    {
        tcs = new TaskCompletionSource<string>();
        showActionSheet(title, allowedFileTypes, allowedFileTypes.Length, Marshal.GetFunctionPointerForDelegate((Action<IntPtr>)OnFileSelectediOS));
        return tcs.Task;
    }

    [AOT.MonoPInvokeCallback(typeof(Action<IntPtr>))]
    private static void OnFileSelectediOS(IntPtr pathPtr)
    {
        string path = Marshal.PtrToStringAnsi(pathPtr);
        tcs.SetResult(path);
    }
#endif

    #endregion
}