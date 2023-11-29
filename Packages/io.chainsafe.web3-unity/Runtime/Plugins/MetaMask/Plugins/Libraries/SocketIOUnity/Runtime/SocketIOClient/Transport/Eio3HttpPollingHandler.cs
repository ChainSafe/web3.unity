using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine.Networking;

namespace MetaMask.SocketIOClient.Transport
{
    public class Eio3HttpPollingHandler : HttpPollingHandler
    {
        public Eio3HttpPollingHandler() : base() { }

        public override Task PostAsync(string uri, IEnumerable<byte[]> bytes, CancellationToken cancellationToken)
        {
            var list = new List<byte>();
            foreach (var item in bytes)
            {
                list.Add(1);
                var length = SplitInt(item.Length + 1).Select(x => (byte)x);
                list.AddRange(length);
                list.Add(byte.MaxValue);
                list.Add(4);
                list.AddRange(item);
            }
            var tcs = new TaskCompletionSource<bool>();
            UnityThread.executeCoroutine(PostCoroutine(uri, list, tcs));
            return tcs.Task;

            //var content = new ByteArrayContent(list.ToArray());
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            //await HttpClient.PostAsync(AppendRandom(uri), content, cancellationToken).ConfigureAwait(false);
        }

        private IEnumerator PostCoroutine(string uri, List<byte> list, TaskCompletionSource<bool> tcs)
        {
            string requestUri = AppendTimestamp(uri);
            var form = new List<IMultipartFormSection>();
            form.Add(new MultipartFormDataSection(list.ToArray()));
            var uwr = UnityWebRequest.Post(requestUri, form);
            uwr.SetRequestHeader("Content-Type", "application/octet-stream");

            yield return uwr.SendWebRequest();
            tcs.SetResult(true);
        }

        private List<int> SplitInt(int number)
        {
            List<int> list = new List<int>();
            while (number > 0)
            {
                list.Add(number % 10);
                number /= 10;
            }
            list.Reverse();
            return list;
        }

        protected override void ProduceText(string text)
        {
            int p = 0;
            while (true)
            {
                int index = text.IndexOf(':', p);
                if (index == -1)
                {
                    break;
                }
                if (int.TryParse(text.Substring(p, index - p), out int length))
                {
                    string msg = text.Substring(index + 1, length);
                    TextSubject.OnNext(msg);
                }
                else
                {
                    break;
                }
                p = index + length + 1;
                if (p >= text.Length)
                {
                    break;
                }
            }
        }

        public override Task PostAsync(string uri, string content, CancellationToken cancellationToken)
        {
            content = content.Length + ":" + content;
            return base.PostAsync(uri, content, cancellationToken);
        }
    }
}
