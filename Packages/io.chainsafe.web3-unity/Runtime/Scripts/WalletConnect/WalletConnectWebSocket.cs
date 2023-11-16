#if !UNITY_2022_1_OR_NEWER
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using Newtonsoft.Json;
using UnityEngine;
using WalletConnectSharp.Common;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Events;
using WalletConnectSharp.Events.Model;
using WalletConnectSharp.Network;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Network.Websocket;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Custom web socket implementation for Wallet Connect, based on NativeWebSocket https://github.com/endel/NativeWebSocket.
    /// </summary>
    public class WalletConnectWebSocket : MonoBehaviour, IJsonRpcConnection, IModule
    {
        private const string AddressNotFoundError = "getaddrinfo ENOTFOUND";
        private const string ConnectionRefusedError = "connect ECONNREFUSED";

        private WebSocket webSocket;

        private EventDelegator delegator;

        private string url;

        private bool registering;

        private Guid contextId;

        private TaskCompletionSource<bool> beginConnection;

        private bool connectionStarted;

        private TimeSpan connectionTimeout = TimeSpan.FromSeconds(60);

        /// <summary>
        /// The Url to connect to
        /// </summary>
        public string Url
        {
            get => url;
            set => url = value;
        }

        public bool IsPaused { get; private set; }

        /// <summary>
        /// The name of this websocket connection module
        /// </summary>
        public string Name => "websocket-connection";

        /// <summary>
        /// The context string of this Websocket module
        /// </summary>
        public string Context => contextId.ToString();

        /// <summary>
        /// The EventDelegator this Websocket connection module is using
        /// </summary>
        public EventDelegator Events => delegator;

        /// <summary>
        /// Whether this websocket connection is connected
        /// </summary>
        public bool Connected
        {
            get
            {
                bool connected = webSocket != null && webSocket.State == WebSocketState.Open;
                WCLogger.Log("Websocket Connected State: " + connected);
                return connected;
            }
        }

        /// <summary>
        /// Whether this websocket connection is currently connecting
        /// </summary>
        public bool Connecting => registering;

        public async void Dispose()
        {
            if (Connected)
            {
                WCLogger.Log("Socket is being disposed, cleanup");
                await Close();
            }
        }

        /// <summary>
        /// Open Socket.
        /// </summary>
        public async Task Open()
        {
            await Register(this.url);
        }

        /// <summary>
        /// Open Socket.
        /// </summary>
        /// <param name="options">Socket options.</param>
        /// <typeparam name="T">Options type.</typeparam>
        /// <returns></returns>
        public Task Open<T>(T options)
        {
            if (typeof(string).IsAssignableFrom(typeof(T)))
            {
                return Register(options as string);
            }

            return Open();
        }

        /// <summary>
        /// Close socket.
        /// </summary>
        /// <exception cref="IOException">Throws if socket is already closed.</exception>
        public async Task Close()
        {
            if (webSocket == null) throw new IOException("Connection already closed");

            await webSocket.Close();

            WCLogger.Log("Closing websocket due to Close() being called");
            OnClose(WebSocketCloseCode.Normal);
        }

        /// <summary>
        /// Send request using socket.
        /// </summary>
        /// <param name="requestPayload">Payload to be sent.</param>
        /// <param name="context">Socket context.</param>
        /// <typeparam name="T">Payload Type.</typeparam>
        public async Task SendRequest<T>(IJsonRpcRequest<T> requestPayload, object context)
        {
            if (webSocket == null) webSocket = await Register(this.url);

            try
            {
                Debug.Log($"[WCWebSocket-{contextId}] Sending request {JsonConvert.SerializeObject(requestPayload)}");

                await webSocket.SendText(JsonConvert.SerializeObject(requestPayload));
            }
            catch (Exception e)
            {
                Debug.LogError(e);

                OnError<T>(requestPayload, e);
            }
        }

        /// <summary>
        /// Send result using socket.
        /// </summary>
        /// <param name="responsePayload">Payload to be sent.</param>
        /// <param name="context">Socket context.</param>
        /// <typeparam name="T">Payload Type.</typeparam>
        public async Task SendResult<T>(IJsonRpcResult<T> responsePayload, object context)
        {
            if (webSocket == null) webSocket = await Register(this.url);

            try
            {
                await webSocket.SendText(JsonConvert.SerializeObject(responsePayload));
            }
            catch (Exception e)
            {
                OnError<T>(responsePayload, e);
            }
        }

        /// <summary>
        /// Send error using socket.
        /// </summary>
        /// <param name="errorPayload">Payload to be sent.</param>
        /// <param name="context">Socket context.</param>
        public async Task SendError(IJsonRpcError errorPayload, object context)
        {
            if (webSocket == null) webSocket = await Register(this.url);

            try
            {
                await webSocket.SendText(JsonConvert.SerializeObject(errorPayload));
            }
            catch (Exception e)
            {
                OnError<object>(errorPayload, e);
            }
        }

        private void Awake()
        {
            contextId = Guid.NewGuid();
            delegator = new EventDelegator(this);
        }

        private async Task<WebSocket> Register(string newUrl)
        {
            if (!Validation.IsWsUrl(newUrl))
            {
                throw new ArgumentException("Provided URL is not compatible with WebSocket connection: " + newUrl);
            }

            if (registering)
            {
                TaskCompletionSource<WebSocket> registeringTask =
                    new TaskCompletionSource<WebSocket>(TaskCreationOptions.None);

                Events.ListenForOnce(WebsocketConnectionEvents.RegisterError,
                    delegate(object sender, GenericEvent<Exception> @event)
                    {
                        registeringTask.SetException(@event.EventData);
                    });

                Events.ListenForOnce(WebsocketConnectionEvents.Open,
                    delegate(object sender, GenericEvent<WebSocket> @event)
                    {
                        registeringTask.SetResult(@event.EventData);
                    });

                await registeringTask.Task;

                return registeringTask.Task.Result;
            }

            this.url = newUrl;
            this.registering = true;

            try
            {
                webSocket = new WebSocket(this.url);
                //_socket = new WebsocketClient(new Uri(_url));

                await StartWebsocket(webSocket).WithTimeout(connectionTimeout, "Unavailable WS RPC url at " + this.url);
                OnOpen(webSocket);
                return webSocket;
            }
            catch (Exception e)
            {
                Events.Trigger(WebsocketConnectionEvents.RegisterError, e);

                WCLogger.Log($"Calling close due to exception {e}");
                OnClose(WebSocketCloseCode.ServerError);

                throw;
            }
        }

        private Task StartWebsocket(WebSocket socket)
        {
            this.webSocket = socket;
            connectionStarted = false;
            beginConnection = new TaskCompletionSource<bool>();

            return beginConnection.Task;
        }

        private void OnOpen(WebSocket socket)
        {
            socket.OnMessage += OnPayload;
            socket.OnClose += OnDisconnect;

            this.registering = false;
            Events.Trigger(WebsocketConnectionEvents.Open, this.webSocket);
        }

        private void OnDisconnect(WebSocketCloseCode code)
        {
            if (code != WebSocketCloseCode.Normal) Events.Trigger(WebsocketConnectionEvents.Error, code);

            WCLogger.Log("Socket closing due to Disconnect event from socket");
            OnClose(code);
        }

        private void OnClose(WebSocketCloseCode code)
        {
            if (this.webSocket == null) return;

            //_socket.Dispose();
            this.webSocket = null;
            this.registering = false;
            Events.Trigger(WebsocketConnectionEvents.Close, code);
        }

        private void OnPayload(byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);

            Debug.Log($"[WCWebSocket-{contextId}] Got payload {json}");

            if (string.IsNullOrWhiteSpace(json)) return;

            Debug.Log($"[WCWebsocket-{contextId}] Triggering payload event with JSON {json}");

            Events.Trigger(WebsocketConnectionEvents.Payload, json);
        }

        void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            if (webSocket != null) webSocket.DispatchMessageQueue();
#endif
        }

        private async void LateUpdate()
        {
            if (connectionStarted || beginConnection == null || beginConnection.Task.IsCompleted) return;
            if (webSocket == null) return;

            connectionStarted = true;
            this.webSocket.OnOpen += SocketOnOnOpen;
            try
            {
                await this.webSocket.Connect();
            }
            catch (Exception e)
            {
                this.beginConnection.TrySetException(e);
            }
        }

        private void SocketOnOnOpen()
        {
            beginConnection.TrySetResult(true);
        }

        private void OnError<T>(IJsonRpcPayload payload, Exception e)
        {
            var exception = e.Message.Contains(AddressNotFoundError) || e.Message.Contains(ConnectionRefusedError)
                ? new IOException("Unavailable WS RPC url at " + this.url)
                : e;

            string message = exception.Message;

            var response = new JsonRpcResponse<T>(payload.Id,
                new Error() { Code = exception.HResult, Data = null, Message = message }, default(T));

            //Trigger the payload event, converting the new JsonRpcResponse object to JSON string
            Events.Trigger(WebsocketConnectionEvents.Payload, JsonConvert.SerializeObject(response));

            Debug.LogError(e);
        }

        private void OnApplicationPause(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}
#endif