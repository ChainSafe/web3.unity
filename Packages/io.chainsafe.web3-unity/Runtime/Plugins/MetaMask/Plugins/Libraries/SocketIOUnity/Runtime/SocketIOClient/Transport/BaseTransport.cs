﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using MetaMask.SocketIOClient.JsonSerializer;
using MetaMask.SocketIOClient.Messages;
using MetaMask.SocketIOClient.UriConverters;

namespace MetaMask.SocketIOClient.Transport
{
    public abstract class BaseTransport : IObserver<string>, IObserver<byte[]>, IObservable<IMessage>, IDisposable
    {
        public BaseTransport(SocketIOOptions options, IJsonSerializer jsonSerializer)
        {
            Options = options;
            MessageSubject = new Subject<IMessage>();
            JsonSerializer = jsonSerializer;
            UriConverter = new UriConverter();
            this._messageQueue = new Queue<IMessage>();
        }

        private DateTime _pingTime;
        private readonly Queue<IMessage> _messageQueue;

        protected SocketIOOptions Options { get; }
        protected Subject<IMessage> MessageSubject { get; }

        protected IJsonSerializer JsonSerializer { get; }
        protected CancellationTokenSource PingTokenSource { get; private set; }
        protected OpenedMessage OpenedMessage { get; private set; }

        public string Namespace { get; set; }
        public IUriConverter UriConverter { get; set; }

        public async Task SendAsync(IMessage msg, CancellationToken cancellationToken)
        {
            msg.Eio = Options.EIO;
            msg.Protocol = Options.Transport;
            var payload = new Payload
            {
                Text = msg.Write()
            };
            if (msg.OutgoingBytes != null)
            {
                payload.Bytes = msg.OutgoingBytes;
            }
            await SendAsync(payload, cancellationToken).ConfigureAwait(false);
        }

        protected virtual async Task OpenAsync(OpenedMessage msg)
        {
            OpenedMessage = msg;
            if (Options.EIO == 3 && string.IsNullOrEmpty(Namespace))
            {
                return;
            }
            var connectMsg = new ConnectedMessage
            {
                Namespace = Namespace,
                Eio = Options.EIO,
                Query = Options.Query,
            };
            if (Options.EIO == 4)
            {
                if (Options.Auth != null)
                {
                    connectMsg.AuthJsonStr = JsonSerializer.Serialize(new[] { Options.Auth }).Json.TrimStart('[').TrimEnd(']');
                }
            }

            for (int i = 1; i <= 3; i++)
            {
                try
                {
                    await SendAsync(connectMsg, CancellationToken.None).ConfigureAwait(false);
                    break;
                }
                catch (Exception e)
                {
                    if (i == 3)
                        OnError(e);
                    else
                        await Task.Delay(TimeSpan.FromMilliseconds(Math.Pow(2, i) * 100));
                }
            }
        }

        /// <summary>
        /// <para>Eio3 ping is sent by the client</para>
        /// <para>Eio4 ping is sent by the server</para>
        /// </summary>
        /// <param name="cancellationToken"></param>
        private void StartPing(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"[Ping] Interval: {OpenedMessage.PingInterval}");

            //var coroutine = StartPingCoroutine(cancellationToken);
            //SocketIOCouroutineRunner.Instance.RunCoroutine(coroutine);

            Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(OpenedMessage.PingInterval);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    try
                    {
                        var ping = new PingMessage();
                        Debug.WriteLine($"[Ping] Sending");
                        await SendAsync(ping, CancellationToken.None).ConfigureAwait(false);
                        Debug.WriteLine($"[Ping] Has been sent");
                        this._pingTime = DateTime.Now;
                        MessageSubject.OnNext(ping);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"[Ping] Failed to send, {e.Message}");
                        MessageSubject.OnError(e);
                        break;
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private IEnumerator StartPingCoroutine(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //await Task.Delay(OpenedMessage.PingInterval);
                yield return new UnityEngine.WaitForSecondsRealtime(OpenedMessage.PingInterval);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                var ping = new PingMessage();
                Debug.WriteLine($"[Ping] Sending");
                Task task = null;
                try
                {
                    task = SendAsync(ping, CancellationToken.None);
                    task.ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"[Ping] Failed to send, {e.Message}");
                    MessageSubject.OnError(e);
                    break;
                }
                yield return new UnityEngine.WaitUntil(() => task == null || task.IsCompleted);
                Debug.WriteLine($"[Ping] Has been sent");
                this._pingTime = DateTime.Now;
                MessageSubject.OnNext(ping);
            }
        }

        public abstract Task ConnectAsync(Uri uri, CancellationToken cancellationToken);

        public abstract Task DisconnectAsync(CancellationToken cancellationToken);

        public abstract void AddHeader(string key, string val);

        public virtual void Dispose()
        {
            try
            {
                MessageSubject.Dispose();
                this._messageQueue.Clear();
                if (PingTokenSource != null)
                {
                    PingTokenSource.Cancel();
                    PingTokenSource.Dispose();
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        public abstract Task SendAsync(Payload payload, CancellationToken cancellationToken);

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            try
            {
                MessageSubject.OnError(error);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        public void OnNext(string text)
        {
            try
            {
                Debug.WriteLine($"[Receive] {text}");
                var msg = MessageFactory.CreateMessage(Options.EIO, text);
                if (msg == null)
                {
                    return;
                }
                if (msg.BinaryCount > 0)
                {
                    msg.IncomingBytes = new List<byte[]>(msg.BinaryCount);
                    this._messageQueue.Enqueue(msg);
                    return;
                }
                if (msg.Type == MessageType.Opened)
                {
                    OpenAsync(msg as OpenedMessage).ConfigureAwait(false);
                }

                if (Options.EIO == 3)
                {
                    if (msg.Type == MessageType.Connected)
                    {
                        var connectMsg = msg as ConnectedMessage;
                        connectMsg.Sid = OpenedMessage.Sid;
                        if ((string.IsNullOrEmpty(Namespace) && string.IsNullOrEmpty(connectMsg.Namespace)) || connectMsg.Namespace == Namespace)
                        {
                            if (PingTokenSource != null)
                            {
                                PingTokenSource.Cancel();
                            }
                            PingTokenSource = new CancellationTokenSource();
                            StartPing(PingTokenSource.Token);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (msg.Type == MessageType.Pong)
                    {
                        var pong = msg as PongMessage;
                        pong.Duration = DateTime.Now - this._pingTime;
                    }
                }

                MessageSubject.OnNext(msg);

                if (msg.Type == MessageType.Ping)
                {
                    this._pingTime = DateTime.Now;
                    try
                    {
                        SendAsync(new PongMessage(), CancellationToken.None).ConfigureAwait(false);
                        MessageSubject.OnNext(new PongMessage
                        {
                            Eio = Options.EIO,
                            Protocol = Options.Transport,
                            Duration = DateTime.Now - this._pingTime
                        });
                    }
                    catch (Exception e)
                    {
                        OnError(e);
                    }
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        public void OnNext(byte[] bytes)
        {
            Debug.WriteLine($"[Receive] binary message");
            if (this._messageQueue.Count > 0)
            {
                var msg = this._messageQueue.Peek();
                msg.IncomingBytes.Add(bytes);
                if (msg.IncomingBytes.Count == msg.BinaryCount)
                {
                    MessageSubject.OnNext(msg);
                    this._messageQueue.Dequeue();
                }
            }
        }

        public IDisposable Subscribe(IObserver<IMessage> observer)
        {
            return MessageSubject.Subscribe(observer);
        }
    }
}
