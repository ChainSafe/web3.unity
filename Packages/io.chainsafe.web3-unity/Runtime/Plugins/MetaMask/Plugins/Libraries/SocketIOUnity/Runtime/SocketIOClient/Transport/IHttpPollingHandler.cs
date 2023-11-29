using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MetaMask.SocketIOClient.Transport
{
    public interface IHttpPollingHandler : IDisposable
    {
        IObservable<string> TextObservable { get; }
        IObservable<byte[]> BytesObservable { get; }
        void SetDefaultHeader(string key, string val);
        Task GetAsync(string uri, CancellationToken cancellationToken);
        Task SendAsync(Uri uri, CancellationToken cancellationToken);
        Task PostAsync(string uri, string content, CancellationToken cancellationToken);
        Task PostAsync(string uri, IEnumerable<byte[]> bytes, CancellationToken cancellationToken);
    }
}
