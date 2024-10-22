using System;
using System.Runtime.InteropServices;
using AOT;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.Scripting;

namespace ChainSafe.Gaming
{
    public class ClipboardManager : MonoBehaviour
    {
        private static IClipboardHandler _clipboardHandler;

#if (UNITY_WEBGL || UNITY_IOS) && !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void CreateGameObject()
        {
            var go = new GameObject("ClipboardManager", typeof(ClipboardManager));
            DontDestroyOnLoad(go);
        }
#endif
        private void Awake()
        {
            InitializeClipboardHandler();
            _clipboardHandler?.SetTextPasteCallback(OnPaste);
        }

        private void InitializeClipboardHandler()
        {
#if (UNITY_WEBGL || UNITY_IOS) && !UNITY_EDITOR
            _clipboardHandler = new ClipboardHandler();
#else
            _clipboardHandler = null;
#endif
        }

        private void Update()
        {
#if ENABLE_INPUT_SYSTEM
            if ((Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.leftCommandKey.isPressed) && Keyboard.current.vKey.wasPressedThisFrame)
            {
                _clipboardHandler?.Paste();
            }
#else
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)) && Input.GetKeyDown(KeyCode.V))
            {
                _clipboardHandler?.Paste();
            }
#endif
        }


        [MonoPInvokeCallback(typeof(Action))]
        public static void OnPaste(string text)
        {
            var currentGo = EventSystem.current?.currentSelectedGameObject;
            if (currentGo != null &&
                currentGo.TryGetComponent(out TMP_InputField inputField))
                inputField.text = text;
        }

        public static void CopyText(string text)
        {
#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
            _clipboardHandler?.CopyTextToClipboard(text);
#else
            GUIUtility.systemCopyBuffer = text;
#endif
        }
    }

    public delegate void ClipboardPasted(string text);

    public interface IClipboardHandler
    {
        void CopyTextToClipboard(string text);
        void SetTextPasteCallback(ClipboardPasted callback);
        void Paste();
    }
#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
    public class ClipboardHandler : IClipboardHandler
    {
        [DllImport("__Internal")]
        private static extern void SetPasteCallback(ClipboardPasted clipboardPasted);

        [DllImport("__Internal")]
        private static extern void PasteFromClipboard();

        [DllImport("__Internal")]
        private static extern void CopyToClipboard(string text);

        public void CopyTextToClipboard(string text)
        {
            CopyToClipboard(text); 
        }

        public void SetTextPasteCallback(ClipboardPasted callback)
        {
            SetPasteCallback(callback); 
        }

        public void Paste()
        {
            PasteFromClipboard();
        }

    }
#endif
}