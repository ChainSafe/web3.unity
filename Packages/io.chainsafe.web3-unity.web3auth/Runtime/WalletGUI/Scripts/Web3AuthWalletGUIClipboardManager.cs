using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Web3AuthWalletGUIClipboardManager
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
        EventSystem system = EventSystem.current;
        
        GameObject selectedObj = system.currentSelectedGameObject;
        
        if (selectedObj != null && selectedObj.TryGetComponent(out TMP_InputField selectedInput))
        {
            if (!selectedInput.isFocused)
            {
                Debug.LogError("Selected InputField is not focused.");
                
                return;
            }
            
            selectedInput.text = text;
        }

        else
        {
            Debug.LogError("No InputField selected to paste text into.");
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