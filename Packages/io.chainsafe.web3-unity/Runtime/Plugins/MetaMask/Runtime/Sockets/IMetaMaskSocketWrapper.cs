using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetaMask.Sockets
{

    /// <summary>
    /// Provides a wrapper for any type of socket.io implementation to be used.
    /// </summary>
    public interface IMetaMaskSocketWrapper : IDisposable
    {

        /// <summary>
        /// Occurs when the socket is connected.
        /// </summary>
        event EventHandler Connected;

        /// <summary>
        /// Occurs when the socket is disconnected.
        /// </summary>
        event EventHandler Disconnected;

        /// <summary>
        /// Initializes the socket.
        /// </summary>
        /// <param name="url">The socket URL</param>
        /// <param name="options">The socket options</param>
        void Initialize(string url, MetaMaskSocketOptions options);

        /// <summary>
        /// Connects the socket..
        /// </summary>
        Task ConnectAsync();

        /// <summary>
        /// Disconnects the socket.
        /// </summary>
        Task DisconnectAsync();

        /// <summary>
        /// Emits a new message through the socket.
        /// </summary>
        /// <param name="eventName">The event name</param>
        /// <param name="data">The data</param>
        void Emit(string eventName, params object[] data);

        /// <summary>
        /// Subscribes for the <paramref name="eventName"/> on the socket, and then the <paramref name="callback"/> gets called once that event occurs.
        /// </summary>
        /// <param name="eventName">The event name</param>
        /// <param name="callback">The callback to be called when the event occurs</param>
        void On(string eventName, Action<string> callback);

        //void OnAny(Action<string> callback);

        /// <summary>
        /// Unsubscribes from the <paramref name="eventName"/> using the <paramref name="callback"/>.
        /// </summary>
        /// <param name="eventName">The event name</param>
        /// <param name="callback">The callback to remove</param>
        void Off(string eventName, Action<string> callback = null);

        Task<(string Response, bool IsSuccessful, string Error)> SendWebRequest(string url, string data, Dictionary<string, string> headers);

        //void OffAny(Action<string> callback = null);

    }

    /// <summary>
    /// The socket options.
    /// </summary>
    public class MetaMaskSocketOptions
    {

        /// <summary>
        /// Gets or sets the extra headers to be used by the socket.io.
        /// </summary>
        public Dictionary<string, string> ExtraHeaders { get; set; }

    }
}
