using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Web3AuthWalletGUIClipboardManager : MonoBehaviour
{
    #region Fields

    #if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);

    [DllImport("__Internal")]
    private static extern string PasteFromClipboard();
    #endif

    #endregion

    #region Methods

    public void OnPasteWebGL(string text)
    {
        var inputFields = FindObjectsOfType<TMP_InputField>();
        TMP_InputField focusedInputField = inputFields.FirstOrDefault(inputField => inputField.isFocused);
        if (focusedInputField != null)
        {
            focusedInputField.text = text;
        }
    }

    private void PasteText()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
        {
        #if UNITY_WEBGL && !UNITY_EDITOR
            PasteFromClipboard();
        #elif UNITY_IOS && !UNITY_EDITOR
            string text = PasteFromClipboard();
            OnPasteWebGL(text);
        #else
            OnPasteWebGL(GUIUtility.systemCopyBuffer);
        #endif
        }
    }

    public static void CopyText(string text)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        CopyToClipboard(text);
        #elif UNITY_IOS && !UNITY_EDITOR
        CopyToClipboard(text);
        #else
        GUIUtility.systemCopyBuffer = text;
        #endif
    }

    private void Update()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        PasteText();
        #endif
    }

    #endregion
}