using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NativeWebSocket;

namespace MetaMask.NativeWebSocket
{

    public class AndroidWebSocket : AndroidJavaProxy, IWebSocket
    {

        #region Events

        public event WebSocketCloseEventHandler OnClose;
        public event WebSocketErrorEventHandler OnError;
        public event WebSocketOpenEventHandler OnOpen;
        public event WebSocketMessageEventHandler OnMessage;
        public event WebSocketTextMessageEventHandler OnTextMessage;

        #endregion

        #region Constants

        private const string Namespace = "io.metamask.unity.websocket";
        private const string WebSocketListenerName = Namespace + ".IUnityWebSocketListener";
        private const string HandlerClassName = Namespace + ".UnityWebSocketHandler";

        #endregion

        #region Fields

        private string url;
        private AndroidJavaObject webSocketHandler;
        private AndroidJavaObject unityActivity;

        #endregion

        #region Properties

        public bool IsConnected { get; private set; }

        public WebSocketState State { get; private set; }

        #endregion

        #region Constructors

        public AndroidWebSocket(string url, Dictionary<string, string> headers = null) : this(url, string.Empty, headers)
        {
        }

        public AndroidWebSocket(string url, string subprotocol, Dictionary<string, string> headers = null) : this(url, new List<string>(), headers)
        {
        }

        public AndroidWebSocket(string url, List<string> subprotocols, Dictionary<string, string> headers = null) : base(WebSocketListenerName)
        {
            this.url = url;

            BuildAndroidObjects();
        }

        #endregion

        #region Public Methods

        public Task Connect()
        {
            State = WebSocketState.Connecting;
            webSocketHandler.Call("open");
            return Task.CompletedTask;
        }

        public Task Close()
        {
            State = WebSocketState.Closing;
            webSocketHandler.Call("close");
            return Task.CompletedTask;
        }

        public Task Send(string text)
        {
            webSocketHandler.Call("send", text);
            return Task.CompletedTask;
        }

        public Task Send(byte[] bytes)
        {
            webSocketHandler.Call("send", bytes);
            return Task.CompletedTask;
        }

        #endregion

        #region IUnityWebSocketListener

        public void OnSocketOpen(AndroidJavaObject webSocket, AndroidJavaObject response)
        {
            State = WebSocketState.Open;
            IsConnected = true;
            OnOpen?.Invoke();
        }

        public void OnSocketClosing(AndroidJavaObject webSocket, int code, string reason)
        {
            State = WebSocketState.Closing;
            IsConnected = false;
            Close();
        }

        public void OnSocketClosed(AndroidJavaObject webSocket, int code, string reason)
        {
            State = WebSocketState.Closed;
            IsConnected = false;
            OnClose?.Invoke((WebSocketCloseCode)code);
        }

        public void OnSocketFailure(AndroidJavaObject webSocket, AndroidJavaObject t, AndroidJavaObject response)
        {
            OnError?.Invoke(t.Call<string>("toString"));
        }

        public void OnSocketMessage(AndroidJavaObject webSocket, string text)
        {
            OnTextMessage?.Invoke(text);
        }

        public void OnSocketMessage(AndroidJavaObject webSocket, byte[] bytes)
        {
            OnMessage?.Invoke(bytes);
        }

        #endregion
        
        #region Private Methods

        private void BuildAndroidObjects()
        {
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            webSocketHandler = new AndroidJavaObject(HandlerClassName, unityActivity, this, this.url);
        }
        
        #endregion

    }

}