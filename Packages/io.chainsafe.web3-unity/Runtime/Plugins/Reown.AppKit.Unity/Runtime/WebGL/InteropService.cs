using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using AOT;
using Newtonsoft.Json;
using UnityEngine;
using Reown.AppKit.Unity.Utils;

namespace Reown.AppKit.Unity.WebGl
{
    public class InteropService
    {
        private static readonly Dictionary<int, PendingInteropCall> PendingInteropCalls = new();

        private readonly ExternalMethod _externalMethod;

        private readonly JsonConverter[] _jsonConverts =
        {
            new ByteArrayJsonConverter()
        };

        public InteropService(ExternalMethod externalMethod)
        {
            _externalMethod = externalMethod;
        }

        public async Task<TRes> InteropCallAsync<TReq, TRes>(string methodName, TReq requestParameter, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var tcs = new TaskCompletionSource<object>();

            var id = Guid.NewGuid().GetHashCode();

            var pendingInteropCall = new PendingInteropCall(typeof(TRes), tcs);
            PendingInteropCalls.Add(id, pendingInteropCall);

            var cancellationTokenRegistration = cancellationToken.Register(() =>
            {
                if (!PendingInteropCalls.TryGetValue(id, out var call))
                    return;

                call.TaskCompletionSource.TrySetCanceled();
                PendingInteropCalls.Remove(id);
            });

            try
            {
                string paramStr = null;
                if (!Equals(requestParameter, default(TReq)))
                {
                    if (typeof(TReq) == typeof(string))
                    {
                        paramStr = requestParameter as string;
                    }
                    else
                    {
                        paramStr = JsonConvert.SerializeObject(requestParameter, _jsonConverts);
                    }
                }

                _externalMethod(id, methodName, paramStr, TcsCallback);

                var result = await tcs.Task;
                return (TRes)result;
            }
            finally
            {
                await cancellationTokenRegistration.DisposeAsync();
            }
        }

        [MonoPInvokeCallback(typeof(ExternalMethodCallback))]
        public static void TcsCallback(int id, string responseData, string responseError = null)
        {
            if (!PendingInteropCalls.TryGetValue(id, out var pendingCall))
            {
                Debug.LogError("No pending call found for id: " + id);
                return;
            }

            if (!string.IsNullOrEmpty(responseError))
            {
                try
                {
                    var error = JsonConvert.DeserializeObject<InteropCallError>(responseError);
                    pendingCall.TaskCompletionSource.SetException(new InteropException(error.message));
                    PendingInteropCalls.Remove(id);
                    return;
                }
                catch (Exception)
                {
                    pendingCall.TaskCompletionSource.SetException(new FormatException($"Unable to parse error response: {responseError}"));
                    PendingInteropCalls.Remove(id);
                    return;
                }
            }
            
            if (responseData == null)
            {
                pendingCall.TaskCompletionSource.SetResult(null);
                PendingInteropCalls.Remove(id);
                return;
            }

            object res = null;
            if (pendingCall.ResType == typeof(string))
            {
                res = responseData.Trim('"');
            }
            else if (pendingCall.ResType == typeof(int) && int.TryParse(responseData, out var intResult))
            {
                res = intResult;
            }
            else if (pendingCall.ResType == typeof(float) && float.TryParse(responseData, out var floatResult))
            {
                res = floatResult;
            }
            else if (pendingCall.ResType == typeof(double) && double.TryParse(responseData, out var doubleResult))
            {
                res = doubleResult;
            }
            else if (pendingCall.ResType == typeof(bool) && bool.TryParse(responseData, out var boolResult))
            {
                res = boolResult;
            }
            else if (pendingCall.ResType == typeof(char) && char.TryParse(responseData, out var charResult))
            {
                res = charResult;
            }
            else if (pendingCall.ResType == typeof(BigInteger) && BigInteger.TryParse(responseData, out var bigIntResult))
            {
                res = bigIntResult;
            }
            else if (pendingCall.ResType != typeof(void))
            {
                try
                {
                    res = JsonConvert.DeserializeObject(responseData, pendingCall.ResType);
                }
                catch (Exception e)
                {
                    pendingCall.TaskCompletionSource.SetException(e);
                    PendingInteropCalls.Remove(id);
                    return;
                }
            }

            pendingCall.TaskCompletionSource.SetResult(res);
            PendingInteropCalls.Remove(id);
        }

        public delegate void ExternalMethod(int id, string methodName, string parameter, ExternalMethodCallback callback);

        public delegate void ExternalMethodCallback(int id, string responseData, string responseError = null);

        private readonly struct PendingInteropCall
        {
            public readonly Type ResType;
            public readonly TaskCompletionSource<object> TaskCompletionSource;

            public PendingInteropCall(Type resType, TaskCompletionSource<object> taskCompletionSource)
            {
                ResType = resType;
                TaskCompletionSource = taskCompletionSource;
            }
        }
    }
}