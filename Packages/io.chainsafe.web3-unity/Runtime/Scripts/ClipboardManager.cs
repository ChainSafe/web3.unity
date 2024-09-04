using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ChainSafe.Gaming
{
#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
    public class ClipboardManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void CreateGameObject()
        {
            var go = new GameObject("WebGLClipboardManager", typeof(ClipboardManager));
            DontDestroyOnLoad(go);
        }
        
    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);

    [DllImport("__Internal")]
    private static extern string PasteFromClipboard();
        

        public void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
            {
            #if UNITY_IOS
                string text = PasteFromClipboard();
                OnPaste(text);
            #else 
                PasteFromClipboard();
            #endif
            }
        }

        public void OnPaste(string text)
        {
            var currentGo = EventSystem.current.currentSelectedGameObject;
            if (currentGo != null && currentGo.TryGetComponent<TMP_InputField>(out var inputField))
            {
                inputField.text = text;
            }
        }
    }
#endif
}