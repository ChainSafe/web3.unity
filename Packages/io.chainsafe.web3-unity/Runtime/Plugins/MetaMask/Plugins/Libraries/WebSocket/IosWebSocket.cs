#if UNITY_IOS
using AOT;

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;
#endif

namespace MetaMask.NativeWebSocket
{

#if UNITY_IOS
    public class IosWebSocket : IWebSocket
    {

    #region Delegates

        delegate void TextMessageReceivedCallback(int instanceId, string message);
        delegate void BinaryMessageReceivedCallback(int instanceId, byte[] message);
        delegate void OpenedCallback(int instanceId);
        delegate void ClosedCallback(int instanceId, int code);
        delegate void ErrorCallback(int instanceId, string error);

    #endregion

    #region Events

        public event WebSocketOpenEventHandler OnOpen;
        public event WebSocketMessageEventHandler OnMessage;
        public event WebSocketTextMessageEventHandler OnTextMessage;
        public event WebSocketErrorEventHandler OnError;
        public event WebSocketCloseEventHandler OnClose;

    #endregion

    #region Fields

        private static Dictionary<int, IosWebSocket> instances = new Dictionary<int, IosWebSocket>();

        protected int instanceId;

    #endregion

    #region Properties

        public bool IsConnected { get; private set; }

        public WebSocketState State { get; private set; }

    #endregion

    #region Native API

        [DllImport("__Internal")]
        static extern int UnityWebSocket_Create(string url);

        [DllImport("__Internal")]
        static extern void UnityWebSocket_Connect(int instanceId);

        [DllImport("__Internal")]
        static extern void UnityWebSocket_Disconnect(int instanceId);

        [DllImport("__Internal")]
        static extern void UnityWebSocket_SendText(int instanceId, string text);

        [DllImport("__Internal")]
        static extern void UnityWebSocket_SendBytes(int instanceId, byte[] bytes, int size);

        [DllImport("__Internal")]
        static extern void UnityWebSocket_SetTextMessageReceivedCallback(int instanceId, TextMessageReceivedCallback callback);

        [DllImport("__Internal")]
        static extern void UnityWebSocket_SetBinaryMessageReceivedCallback(int instanceId, BinaryMessageReceivedCallback callback);

        [DllImport("__Internal")]
        static extern void UnityWebSocket_SetOpenCallback(int instanceId, OpenedCallback callback);

        [DllImport("__Internal")]
        static extern void UnityWebSocket_SetCloseCallback(int instanceId, ClosedCallback callback);

        [DllImport("__Internal")]
        static extern void UnityWebSocket_SetErrorCallback(int instanceId, ErrorCallback callback);

    #endregion

    #region Constructors

        public IosWebSocket(string url, Dictionary<string, string> headers = null) : this(url, string.Empty, headers)
        {
        }

        public IosWebSocket(string url, string subprotocol, Dictionary<string, string> headers = null) : this(url, new List<string>(), headers)
        {
        }

        public IosWebSocket(string url, List<string> subprotocols, Dictionary<string, string> headers = null)
        {
            this.instanceId = UnityWebSocket_Create(url);
            instances[instanceId] = this;
        }

    #endregion

    #region Public Methods

        public Task Connect()
        {
            UnityWebSocket_SetTextMessageReceivedCallback(this.instanceId, OnTextMessageReceivedCallback);
            UnityWebSocket_SetBinaryMessageReceivedCallback(this.instanceId, OnBinaryMessageReceivedCallback);
            UnityWebSocket_SetOpenCallback(this.instanceId, OnOpenedCallback);
            UnityWebSocket_SetCloseCallback(this.instanceId, OnClosedCallback);
            UnityWebSocket_SetErrorCallback(this.instanceId, OnErrorCallback);

            State = WebSocketState.Connecting;
            UnityWebSocket_Connect(this.instanceId);

            return Task.CompletedTask;
        }

        public Task Close()
        {
            State = WebSocketState.Closing;
            UnityWebSocket_Disconnect(this.instanceId);

            return Task.CompletedTask;
        }

        public Task Send(string text)
        {
            UnityWebSocket_SendText(this.instanceId, text);

            return Task.CompletedTask;
        }

        public Task Send(byte[] bytes)
        {
            UnityWebSocket_SendBytes(this.instanceId, bytes, bytes.Length);

            return Task.CompletedTask;
        }

    #endregion

    #region Callbacks

        [MonoPInvokeCallback(typeof(TextMessageReceivedCallback))]
        static void OnTextMessageReceivedCallback(int instanceId, string message)
        {
            if (instances[instanceId] != null) {
                instances[instanceId].OnInstanceTextMessageReceived(message);
            }
        }

        void OnInstanceTextMessageReceived(string message)
        {
            OnTextMessage?.Invoke(message);
        }

        [MonoPInvokeCallback(typeof(BinaryMessageReceivedCallback))]
        static void OnBinaryMessageReceivedCallback(int instanceId, byte[] message)
        {
            if (instances[instanceId] != null)
            {
                instances[instanceId].OnInstanceBinaryMessageReceived(message);
            }
        }

        void OnInstanceBinaryMessageReceived(byte[] message)
        {
            OnMessage?.Invoke(message);
        }

        [MonoPInvokeCallback(typeof(OpenedCallback))]
        static void OnOpenedCallback(int instanceId)
        {
            if (instances[instanceId] != null)
            {
                instances[instanceId].OnInstanceOpened();
            }
        }

        void OnInstanceOpened()
        {
            IsConnected = true;
            State = WebSocketState.Open;
            OnOpen?.Invoke();
        }

        [MonoPInvokeCallback(typeof(ClosedCallback))]
        static void OnClosedCallback(int instanceId, int code)
        {
            if (instances[instanceId] != null)
            {
                instances[instanceId].OnInstanceClosed(code);
            }
        }

        void OnInstanceClosed(int code)
        {
            UnityWebSocket_SetTextMessageReceivedCallback(this.instanceId, null);
            UnityWebSocket_SetBinaryMessageReceivedCallback(this.instanceId, null);
            UnityWebSocket_SetOpenCallback(this.instanceId, null);
            UnityWebSocket_SetErrorCallback(this.instanceId, null);
            UnityWebSocket_SetCloseCallback(this.instanceId, null);

            IsConnected = false;
            State = WebSocketState.Closed;
            OnClose?.Invoke((WebSocketCloseCode)code);
        }

        [MonoPInvokeCallback(typeof(ErrorCallback))]
        static void OnErrorCallback(int instanceId, string error)
        {
            if (instances[instanceId] != null)
            {
                instances[instanceId].OnInstanceError(error);
            }
        }

        void OnInstanceError(string error)
        {
            OnError?.Invoke(error);
        }

    #endregion

    }
#endif

}