using System;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Reown.Core.Common.Logging;
using Reown.Sign.Unity;

namespace Reown.Sign.Nethereum.Unity
{
    public class ReownSignUnityInterceptor : RequestInterceptor
    {
        public readonly ReownInterceptor ReownInterceptor;
        public readonly SignClientUnity SignClient;

        public ReownSignUnityInterceptor(SignClientUnity signClient, ReownInterceptor reownInterceptor)
        {
            SignClient = signClient;
            ReownInterceptor = reownInterceptor;
        }

        public ReownSignUnityInterceptor(SignClientUnity signClient)
        {
            SignClient = signClient;
            ReownInterceptor = new ReownInterceptor(new ReownSignServiceCore(SignClient));
        }

        public override Task<object> InterceptSendRequestAsync<T>(
            Func<RpcRequest, string, Task<T>> interceptedSendRequestAsync,
            RpcRequest request,
            string route = null)
        {
            var result = ReownInterceptor.InterceptSendRequestAsync(interceptedSendRequestAsync, request, route);

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            try
            {
                if (ReownInterceptor.SignMethods.Contains(request.Method))
                {
                    var activeSessionTopic = SignClient.AddressProvider.DefaultSession.Topic;
                    SignClient.Linker.OpenSessionRequestDeepLinkAfterMessageFromSession(activeSessionTopic);
                }
            }
            catch (Exception e)
            {
                ReownLogger.LogError(e);
            }
#endif

            return result;
        }

        public override Task<object> InterceptSendRequestAsync<T>(
            Func<string, string, object[], Task<T>> interceptedSendRequestAsync,
            string method,
            string route = null,
            params object[] paramList)
        {
            var result = ReownInterceptor.InterceptSendRequestAsync(interceptedSendRequestAsync, method, route, paramList);

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            try
            {
                if (ReownInterceptor.SignMethods.Contains(method))
                {
                    var activeSessionTopic = SignClient.AddressProvider.DefaultSession.Topic;
                    SignClient.Linker.OpenSessionRequestDeepLinkAfterMessageFromSession(activeSessionTopic);
                }
            }
            catch (Exception e)
            {
                ReownLogger.LogError(e);
            }
#endif

            return result;
        }
    }
}