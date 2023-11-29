using System;
using System.Collections.Generic;
using System.Text;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
#endif
using System.Threading.Tasks;

using MetaMask.SocketIOClient;
using UnityEngine;

using UnityEngine.Networking;

namespace MetaMask.Sockets
{

    public class MetaMaskUnitySocketIO : IMetaMaskSocketWrapper
    {

#if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>The name of the Socket.IO game object.</summary>
        private static readonly string SocketGameObjectName = "SocketIo_Ref";
#endif

        /// <summary>The protocol version.</summary>
        private static byte _protocol = 0;
        /// <summary>The protocol version.</summary>
        public static byte protocol
        {
            get
            {
                if (_protocol == 0)
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    _protocol = GetProtocol();
#else
                    _protocol = 5;
#endif
                }
                return _protocol;
            }
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>Gets the socket for the given id.</summary>
        /// <param name="id">The id of the socket.</param>
        /// <returns>The socket for the given id.</returns>
        private static Dictionary<int, MetaMaskUnitySocketIO> EnabledSockets = new Dictionary<int, MetaMaskUnitySocketIO>();
#endif

        /// <summary>Gets the next socket ID.</summary>
        /// <returns>The next socket ID.</returns>
        private static int LastSocketId = -1;

        /// <summary>Raised when the connection to the server is established.</summary>
        public event EventHandler Connected;

        /// <summary>Raised when the socket has been disconnected.</summary>
        public event EventHandler Disconnected;

        /// <summary>The socket.</summary>
        protected SocketIOUnity socket;
        /// <summary>The socket ID of the socket that this instance is associated with.</summary>
        protected int socketId;
#if UNITY_WEBGL && !UNITY_EDITOR
        private event Action<string> Action_AnyEvents;
        private Dictionary<string, List<Action<string>>> ActionEvents = new Dictionary<string, List<Action<string>>>();
#endif

        public SocketIO Socket => this.socket;

        /// <summary>Creates a new MetaMaskUnitySocketIO instance.</summary>
        public MetaMaskUnitySocketIO()
        {
            LastSocketId++;
            this.socketId = LastSocketId;
        }

        public async Task<(string Response, bool IsSuccessful, string Error)> SendWebRequest(string url, string data, Dictionary<string, string> headers)
        {
            using var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST, new DownloadHandlerBuffer(), new UploadHandlerRaw(Encoding.UTF8.GetBytes(data)));

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    uwr.SetRequestHeader(header.Key, header.Value);
                }
            }
            await uwr.SendWebRequest();

            return (uwr.downloadHandler.text, uwr.result == UnityWebRequest.Result.Success, uwr.error);
        }

        /// <summary>Initializes the socket.</summary>0
        /// <param name="url">The URL of the socket.</param>
        /// <param name="options">The options for the socket.</param>
        public void Initialize(string url, MetaMaskSocketOptions options)
        {
            var socketOptions = new SocketIOOptions();
            socketOptions.ExtraHeaders = options.ExtraHeaders;

            this.socket = new SocketIOUnity(url, socketOptions);

            this.socket.OnConnected += OnSocketConnected;
            this.socket.OnDisconnected += OnSocketDisconnected;
#if UNITY_WEBGL && !UNITY_EDITOR
            //check for gameobject
            if (GameObject.Find(SocketGameObjectName) == null)
            {
                GameObject go = new GameObject(SocketGameObjectName);
                go.AddComponent<SocketIoInterface>();

                GameObject.DontDestroyOnLoad(go);

                SetupGameObjectName(SocketGameObjectName);
            }

            // TODO: Pass options for WebGL socket.io
            this.socketId = EstablishSocket(url, string.Empty);

            EnabledSockets.Add(this.socketId, this);
#endif
        }

        private void OnSocketDisconnected(object sender, string e)
        {
            Debug.Log(e);
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        private void OnSocketConnected(object sender, EventArgs e)
        {
            Connected?.Invoke(this, e);
        }

        /// <summary>Connects to the server.</summary>
        /// <returns>A task that represents the asynchronous connect operation.</returns>
        public Task ConnectAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Socket_Connect(this.socketId);
            return Task.CompletedTask;
#else
            this.socket.Connect();
            return Task.CompletedTask;
#endif
        }

        /// <summary>Disconnects the socket.</summary>
        public Task DisconnectAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Socket_Disconnect(this.socketId);
            return Task.CompletedTask;
#else
            if (this.socket != null)
                this.socket.Disconnect();
            return Task.CompletedTask;
#endif
        }

        /// <summary>Disposes of the socket.</summary>
        public void Dispose()
        {
            if (this.socket != null)
                this.socket.Dispose();
        }

        /// <summary>Emit an event to the server.</summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="data">The data to send with the event.</param>
        public void Emit(string eventName, params object[] data)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (data == null)
            {
                Socket_Emit(this.socketId, eventName, null);
            }
            else
            {
                string result;
                    result = JsonConvert.SerializeObject(data[0]);
                Socket_Emit(this.socketId, eventName, result);
            }
#else
            if (this.socket != null)
                this.socket.Emit(eventName, data);
#endif
        }

        /// <summary>Registers a callback for the specified event.</summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="callback">The callback to register.</param>
        public void On(string eventName, Action<string> callback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (!this.ActionEvents.ContainsKey(eventName))
            {
                this.ActionEvents.Add(eventName, new List<Action<string>>());
            }
            this.ActionEvents[eventName].Add(callback);
#else
            this.socket.On(eventName, response =>
            {
                callback(response.ToString());
            });
            //this.socket.OnUnityThread(eventName, response =>
            //{
            //    callback(response.ToString());
            //});
#endif
        }

        /// <summary>Removes the specified callback from the list of callbacks for the specified event.</summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="callback">The callback to remove.</param>
        public void Off(string eventName, Action<string> callback = null)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (callback != null)
            {
                if (this.ActionEvents.TryGetValue(eventName, out List<Action<string>> value))
                {
                    value.Remove(callback);
                }
            }
            else
            {
                this.ActionEvents = new Dictionary<string, List<Action<string>>>();
            }
#else
            this.socket.Off(eventName);
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>Invokes an event.</summary>
        /// <param name="ev">The event name.</param>
        /// <param name="data">The event data.</param>
        public void InvokeEvent(string ev, string data)
        {
            Action_AnyEvents?.Invoke(data);

            if (ev == "connect")
            {
                Connected?.Invoke(this, EventArgs.Empty);
            }

            if (ev == "disconnect")
            {
                //Disconnected?.Invoke(this, EventArgs.Empty);
            }

            //invoke event specific events
            if (this.ActionEvents.TryGetValue(ev, out List<Action<string>> value))
            {
                foreach (Action<string> act in value)
                {
                    act.Invoke(data);
                }
            }
        }

        /// <summary>Gets the protocol of the current application.</summary>
        /// <returns>The protocol of the current application.</returns>
        [DllImport("__Internal")]
        private static extern byte GetProtocol();

        /// <summary>Establishes a socket connection to the specified URL.</summary>
        /// <param name="url">The URL to connect to.</param>
        /// <param name="options">The options to use when establishing the connection.</param>
        /// <returns>The socket ID.</returns>
        [DllImport("__Internal")]
        private static extern int EstablishSocket(string url, string options);

        /// <summary>Sets up the name of the game object.</summary>
        /// <param name="name">The name of the game object.</param>
        /// <returns>The name of the game object.</returns>
        [DllImport("__Internal")]
        private static extern string SetupGameObjectName(string name);

        /// <summary>Determines whether the socket is connected.</summary>
        /// <param name="id">The socket ID.</param>
        /// <returns>Whether the socket is connected.</returns>
        [DllImport("__Internal")]
        private static extern bool Socket_IsConnected(int id);

        /// <summary>Gets the connection ID of a socket.</summary>
        /// <param name="id">The socket ID.</param>
        /// <returns>The connection ID of the socket.</returns>
        [DllImport("__Internal")]
        private static extern string Socket_Get_Conn_Id(int id);

        /// <summary>Connects to the server.</summary>
        /// <param name="id">The socket ID.</param>
        [DllImport("__Internal")]
        private static extern void Socket_Connect(int id);

        /// <summary>Disconnects the socket.</summary>
        /// <param name="id">The socket's id.</param>
        [DllImport("__Internal")]
        private static extern void Socket_Disconnect(int id);

        // [DllImport("__Internal")]
        // private static extern void Socket_Send(int id, string data);

        /// <summary>Emits an event to the socket.</summary>
        /// <param name="id">The socket ID.</param>
        /// <param name="ev">The event name.</param>
        /// <param name="data">The event data.</param>
        [DllImport("__Internal")]
        private static extern void Socket_Emit(int id, string ev, string data);

        //gameobject for webgl
        public class SocketIoInterface : MonoBehaviour
        {
            /// <summary>Calls the socket event.</summary>
            /// <param name="data">The data.</param>
            public void callSocketEvent(string data)
            {
                //SocketEvent ev = JsonUtility.FromJson<SocketEvent>(data);
                var ev = JsonConvert.DeserializeObject<SocketEvent>(data);
                if (EnabledSockets.TryGetValue(ev.SocketId, out MetaMaskUnitySocketIO soc))
                {
                    soc.InvokeEvent(ev.EventName, ev.JsonData);
                }
                else
                {
                    throw new System.NullReferenceException("socket does not exist");
                }
            }
        }

        private struct SocketEvent
        {
              /// <summary>The Event Name.</summary>
            public string EventName;
             /// <summary>The Socket ID.</summary>
            public int SocketId;
             /// <summary>The JSON Data.</summary>
            public string JsonData;
        }
#endif
    }

}