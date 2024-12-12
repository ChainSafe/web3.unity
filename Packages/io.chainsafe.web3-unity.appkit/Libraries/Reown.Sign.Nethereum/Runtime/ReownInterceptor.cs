using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.HostWallet;

namespace Reown.Sign.Nethereum
{
    public class ReownInterceptor : RequestInterceptor
    {
        private readonly ReownSignService _reownSignService;

        public readonly HashSet<string> SignMethods = new()
        {
            ApiMethods.eth_sendTransaction.ToString(),
            ApiMethods.personal_sign.ToString(),
            ApiMethods.eth_signTypedData_v4.ToString(),
            ApiMethods.wallet_switchEthereumChain.ToString(),
            ApiMethods.wallet_addEthereumChain.ToString()
        };

        public ReownInterceptor(ReownSignService reownSignService)
        {
            _reownSignService = reownSignService;
        }

        public override async Task<object> InterceptSendRequestAsync<T>(
            Func<RpcRequest, string, Task<T>> interceptedSendRequestAsync,
            RpcRequest request,
            string route = null)
        {
            if (!SignMethods.Contains(request.Method))
            {
                return await base
                    .InterceptSendRequestAsync(interceptedSendRequestAsync, request, route)
                    .ConfigureAwait(false);
            }

            if (!_reownSignService.IsWalletConnected)
                throw new InvalidOperationException("[ReownInterceptor] Wallet is not connected");

            if (_reownSignService.IsMethodSupported(request.Method))
            {
                if (request.Method == ApiMethods.eth_sendTransaction.ToString())
                {
                    return await _reownSignService.SendTransactionAsync((TransactionInput)request.RawParameters[0]);
                }

                if (request.Method == ApiMethods.personal_sign.ToString())
                {
                    return await _reownSignService.PersonalSignAsync((string)request.RawParameters[0]);
                }

                if (request.Method == ApiMethods.eth_signTypedData_v4.ToString())
                {
                    // If parameter has only one element, it's a json data.
                    // Otherwise, expect the data to be at index 1
                    var dataIndex = request.RawParameters.Length > 1 ? 1 : 0;
                    
                    return await _reownSignService.EthSignTypedDataV4Async((string)request.RawParameters[dataIndex]);
                }

                if (request.Method == ApiMethods.wallet_switchEthereumChain.ToString())
                {
                    return await _reownSignService.WalletSwitchEthereumChainAsync((SwitchEthereumChainParameter)request.RawParameters[0]);
                }

                if (request.Method == ApiMethods.wallet_addEthereumChain.ToString())
                {
                    return await _reownSignService.WalletAddEthereumChainAsync((AddEthereumChainParameter)request.RawParameters[0]);
                }

                throw new NotImplementedException();
            }

            return await base
                .InterceptSendRequestAsync(interceptedSendRequestAsync, request, route)
                .ConfigureAwait(false);
        }

        public override async Task<object> InterceptSendRequestAsync<T>(
            Func<string, string, object[], Task<T>> interceptedSendRequestAsync,
            string method,
            string route = null,
            params object[] paramList)
        {
            if (!SignMethods.Contains(method))
            {
                return await base
                    .InterceptSendRequestAsync(interceptedSendRequestAsync, method, route, paramList)
                    .ConfigureAwait(false);
            }

            if (!_reownSignService.IsWalletConnected)
                throw new InvalidOperationException("[ReownInterceptor] Wallet is not connected");

            if (_reownSignService.IsMethodSupported(method))
            {
                if (method == ApiMethods.eth_sendTransaction.ToString())
                {
                    return await _reownSignService.SendTransactionAsync((TransactionInput)paramList[0]);
                }

                if (method == ApiMethods.personal_sign.ToString())
                {
                    return await _reownSignService.PersonalSignAsync((string)paramList[0]);
                }

                if (method == ApiMethods.eth_signTypedData_v4.ToString())
                {
                    return await _reownSignService.EthSignTypedDataV4Async((string)paramList[0]);
                }

                if (method == ApiMethods.wallet_switchEthereumChain.ToString())
                {
                    return await _reownSignService.WalletSwitchEthereumChainAsync((SwitchEthereumChainParameter)paramList[0]);
                }

                if (method == ApiMethods.wallet_addEthereumChain.ToString())
                {
                    return await _reownSignService.WalletAddEthereumChainAsync((AddEthereumChainParameter)paramList[0]);
                }

                throw new NotImplementedException();
            }

            return await base
                .InterceptSendRequestAsync(interceptedSendRequestAsync, method, route, paramList)
                .ConfigureAwait(false);
        }
    }
}