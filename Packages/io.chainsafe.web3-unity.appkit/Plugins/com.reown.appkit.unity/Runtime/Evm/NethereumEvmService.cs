using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Contracts.Standards.ERC1271.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Signer.EIP712;
using Nethereum.Util;
using Nethereum.Web3;
using Reown.Sign.Nethereum.Unity;
using Reown.Sign.Unity;
using UnityEngine;
using HexBigInteger = Nethereum.Hex.HexTypes.HexBigInteger;

namespace Reown.AppKit.Unity
{
    public class NethereumEvmService : EvmService
    {
        private readonly Eip712TypedDataSigner _eip712TypedDataSigner = new();

        private readonly EthereumMessageSigner _ethereumMessageSigner = new();
        private ReownSignUnityInterceptor _interceptor;

        public IWeb3 Web3 { get; private set; }

        protected override Task InitializeAsyncCore(SignClientUnity signClient)
        {
            _interceptor = new ReownSignUnityInterceptor(signClient);

            SetInitialWeb3Instance();

            AppKit.ChainChanged += ChainChangedHandler;
            return Task.CompletedTask;
        }
        

        // -- Nethereum Web3 Instance ---------------------------------

        private void ChainChangedHandler(object sender, NetworkController.ChainChangedEventArgs e)
        {
            if (e.NewChain != null)
                UpdateWeb3Instance(e.NewChain.ChainId);
        }

        private void SetInitialWeb3Instance()
        {
            if (Web3 != null)
                return;

            var networkController = AppKit.NetworkController;
            var activeChain = networkController.ActiveChain;
            var chainId = string.Empty;
            if (activeChain != null)
            {
                chainId = activeChain.ChainId;
            }
            else if (networkController.Chains.Values != null && networkController.Chains.Values.Count != 0)
            {
                chainId = networkController.Chains.Values.First().ChainId;
            }

            if (!string.IsNullOrWhiteSpace(chainId))
                UpdateWeb3Instance(chainId);
        }

        private void UpdateWeb3Instance(string chainId)
        {
            Web3 = new Web3(CreateRpcUrl(chainId))
            {
                Client =
                {
                    OverridingRequestInterceptor = _interceptor
                }
            };
        }

        private static string CreateRpcUrl(string chainId)
        {
            return $"https://rpc.walletconnect.com/v1?chainId={chainId}&projectId={AppKit.Config.projectId}";
        }


        // -- Get Balance ----------------------------------------------

        protected override async Task<BigInteger> GetBalanceAsyncCore(string address)
        {
            var hexBigInt = await Web3.Eth.GetBalance.SendRequestAsync(address);
            return hexBigInt.Value;
        }


        // -- Sign Message ---------------------------------------------

        protected override async Task<string> SignMessageAsyncCore(string message)
        {
            var encodedMessage = message.ToHexUTF8();
            return await Web3.Client.SendRequestAsync<string>("personal_sign", null, encodedMessage);
        }


        // -- Verify Message -------------------------------------------

        protected override async Task<bool> VerifyMessageSignatureAsyncCore(string address, string message, string signature)
        {

            // -- ERC-6492
            var erc6492Service = Web3.Eth.SignatureValidationPredeployContractERC6492;
            if (erc6492Service.IsERC6492Signature(signature))
            {
                return await erc6492Service.IsValidSignatureMessageAsync(address, message, signature.HexToByteArray());
            }

            // -- EOA
            var recoveredAddress = _ethereumMessageSigner.EncodeUTF8AndEcRecover(message, signature);
            if (recoveredAddress.IsTheSameAddress(address))
            {
                return true;
            }

            // -- ERC-1271
            var ethGetCode = await Web3.Eth.GetCode.SendRequestAsync(address);
            if (ethGetCode is { Length: > 2 })
            {
                var hashedMessage = _ethereumMessageSigner.HashPrefixedMessage(Encoding.UTF8.GetBytes(message));

                var isValidSignatureFunctionMessage = new IsValidSignatureFunction()
                {
                    Hash = hashedMessage,
                    Signature = signature.HexToByteArray()
                };

                var handler = Web3.Eth.GetContractQueryHandler<IsValidSignatureFunction>();

                try
                {
                    var result = await handler.QueryAsync<byte[]>(address, isValidSignatureFunctionMessage);

                    // The magic value 0x1626ba7e
                    var magicValue = new byte[]
                    {
                        0x16,
                        0x26,
                        0xBA,
                        0x7E
                    };

                    return result != null && result.SequenceEqual(magicValue);
                }
                catch (SmartContractRevertException)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    return false;
                }
            }

            return false;
        }


        // -- Sign Typed Data ------------------------------------------

        protected override Task<string> SignTypedDataAsyncCore(string dataJson)
        {
            return Web3.Client.SendRequestAsync<string>("eth_signTypedData_v4", null, dataJson);
        }


        // -- Verify Typed Data ----------------------------------------

        protected override Task<bool> VerifyTypedDataSignatureAsyncCore(string address, string dataJson, string signature)
        {
            var recoveredAddress = _eip712TypedDataSigner.RecoverFromSignatureV4(dataJson, signature);
            return Task.FromResult(recoveredAddress.IsTheSameAddress(address));
        }


        // -- Read Contract -------------------------------------------

        protected override async Task<TReturn> ReadContractAsyncCore<TReturn>(string contractAddress, string contractAbi, string methodName, object[] arguments = null)
        {
            var contract = Web3.Eth.GetContract(contractAbi, contractAddress);
            var function = contract.GetFunction(methodName);

            return await function.CallAsync<TReturn>(arguments);
        }


        // -- Write Contract ------------------------------------------

        protected override async Task<string> WriteContractAsyncCore(string contractAddress, string contractAbi, string methodName, BigInteger value = default, BigInteger gas = default, params object[] arguments)
        {
            var contract = Web3.Eth.GetContract(contractAbi, contractAddress);
            var function = contract.GetFunction(methodName);

            return await function.SendTransactionAsync(
                null, // will be automatically filled by interceptor
                new HexBigInteger(gas),
                new HexBigInteger(value),
                arguments
            );
        }


        // -- Send Transaction ----------------------------------------

        protected override Task<string> SendTransactionAsyncCore(string addressTo, BigInteger value, string data = null)
        {
            var transactionInput = new TransactionInput(data, addressTo, new HexBigInteger(value));
            return Web3.Client.SendRequestAsync<string>("eth_sendTransaction", null, transactionInput);
        }
        
        
        // -- Send Raw Transaction ------------------------------------

        protected override Task<string> SendRawTransactionAsyncCore(string signedTransaction)
        {
            return Web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(signedTransaction);
        }


        // -- Estimate Gas ---------------------------------------------
        
        protected override async Task<BigInteger> EstimateGasAsyncCore(string addressTo, BigInteger value, string data = null)
        {
            var transactionInput = new TransactionInput(data, addressTo, new HexBigInteger(value));
            return await Web3.Eth.Transactions.EstimateGas.SendRequestAsync(transactionInput);
        }

        protected override async Task<BigInteger> EstimateGasAsyncCore(string contractAddress, string contractAbi, string methodName, BigInteger value = default, params object[] arguments)
        {
            var contract = Web3.Eth.GetContract(contractAbi, contractAddress);
            var function = contract.GetFunction(methodName);
            
            var transactionInput = new TransactionInput(function.GetData(arguments), contractAddress, new HexBigInteger(value));
            return await Web3.Eth.Transactions.EstimateGas.SendRequestAsync(transactionInput);
        }
        
        
        // -- Get Gas Price -------------------------------------------

        protected override async Task<BigInteger> GetGasPriceAsyncCore()
        {
            var hexBigInt = await Web3.Eth.GasPrice.SendRequestAsync();
            return hexBigInt.Value;
        }
    }
}