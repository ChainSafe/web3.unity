using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;

using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Network;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Web3Unity.Scripts.Library.Ethers.Utils;

namespace Web3Unity.Scripts.Prefabs.Ethers
{
    public class ChainSafeRPC 
    {
        private Dictionary<ulong, Chains.Chain> _chains;
        private static BaseProvider _provider;

        private static string NFT_ADDRESS = "0xc81fa2eacc1c45688d481b11ce94c24a136e125d";

        private string NFT_ABI =
            "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"approved\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"getApproved\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"_data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";



        /* public async Task<bool> SwitchNetwork()
        {
            // check type of provider to be Web3Provider
            if (_provider is not Web3Provider provider)
            {
                Debug.Log("Provider is not Web3Provider");
                return false;
            }

            var chain = _chains[5]; // Goerli
            var network = await _provider.GetNetwork();

            if (network.ChainId != chain.ChainId)
            {
                try
                {
                    await provider.Send<string>("wallet_switchEthereumChain",
                        new object[] {new {chainId = "0x" + chain.ChainId.ToString("x")}});
                }
                catch (Web3GLLight.WalletException err)
                {
                    // This error code indicates that the chain has not been added to MetaMask.
                    if (err.code == 4902)
                    {
                        await provider.Send<string>("wallet_addEthereumChain", new object[]
                        {
                            new
                            {
                                chainId = "0x" + chain.ChainId.ToString("x"),
                                chainName = chain.Name,
                                nativeCurrency = new
                                {
                                    name = chain.NativeCurrencyInfo.Name,
                                    symbol = chain.NativeCurrencyInfo.Symbol,
                                    decimals = chain.NativeCurrencyInfo.Decimals
                                },
                                rpcUrls = chain.RPC,
                                blockExplorerUrls = new[] {chain.Explorers[0].Url}
                            }
                        });
                    }
                    else
                    {
                        // re-throw error
                        throw;
                    }
                }
            }

            return true;
        }*/
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetStorageAt(string _address, int position = 0)
        {
            var provider = new JsonRpcProvider("");
            var slot0 = await provider.GetStorageAt(_address, new BigInteger(position));
            Console.WriteLine($"Contract slot 0: {slot0}");

            return slot0;
        }

        /// <summary>
        /// CallContractMethod
        /// </summary>
        /// <returns></returns>
        public async Task<string> CallContractMethod(string _abi, string _contract, string _method)
        {
            var provider = new JsonRpcProvider("");
            var contract = new Contract(_abi, _contract, provider);

            var name = await contract.Call(_method);
            Console.WriteLine($"NFT.name(): {name[0]}");

            var symbol = await contract.Call("symbol");
            Console.WriteLine($"NFT.symbol(): {symbol[0]}");

            var tokenURI = await contract.Call("tokenURI", new object[] { 1 });
            Console.WriteLine($"NFT.tokenURI(1): {tokenURI[0]}");

            var ownerOf = await contract.Call("ownerOf", new object[] { 1 });
            Console.WriteLine($"NFT.owner(1): {ownerOf[0]}");

            var balanceOf = await contract.Call("balanceOf", new object[] { ownerOf[0] });
            Console.WriteLine($"NFT.ownerOf({ownerOf[0]}): {balanceOf[0]}");

            return name[0].ToString();
        }

        public static async Task<string> Call(string abi, string address, string method)
        {
            var provider = new JsonRpcProvider("");
            var contract = new Contract(abi, address, provider);
            string[] obj = { };
            var args = JsonConvert.SerializeObject(obj);
            // var name = await contract.Call(method);
            // Debug.Log($"NFT.name(): {name[0]}");

            //  var symbol = await contract.Call("symbol");
            //  Debug.Log($"NFT.symbol(): {symbol[0]}");

            //  var tokenURI = await contract.Call("tokenURI", new object[] {1});
            //  Debug.Log($"NFT.tokenURI(1): {tokenURI[0]}");

            var ownerOf = await contract.Call("ownerOf", new object[] { 1 });
            var balanceOf = await contract.Call("balanceOf", new object[] { ownerOf[0] });
            var contractData = await contract.Call(method, obj);

            Console.WriteLine($"NFT.ownerOf({contractData[0]}");

            return contractData[0].ToString();
        }

        /// <summary>
        /// EstimateGasContractMethod
        /// </summary>
        /// <returns></returns>
        public async Task<string> EstimateGasContractMethod(string abi, string address, string provider, string method,
            string args)
        {
            var contract = new Contract(abi, address, _provider);
            var result = await contract.EstimateGas("ownerOf", new object[] { 1 });
            Console.WriteLine($"NFT.onwerOf(1) gas: {result}");
            return result.ToString();
        }

        /// <summary>
        /// GetAccounts
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetAccounts()
        {
            if (_provider is not JsonRpcProvider provider)
            {
                Console.WriteLine("Provider is not JsonRpcProvider");
                return false;
            }

            var accounts = await provider.ListAccounts();
            foreach (var account in accounts)
            {
                var accountBalance = await provider.GetBalance(account);
                Console.WriteLine($"{account} balance: {Units.FormatEther(accountBalance)} ETH");
            }

            return true;
        }

        /// <summary>
        /// GetSigner
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetSigner()
        {
            if (_provider is not JsonRpcProvider provider)
            {
                Console.WriteLine("Provider is not JsonRpcProvider");
                return false;
            }

            var signer = provider.GetSigner(); // default signer at index 0
            // var signer = provider.GetSigner(1); // signer at index 1
            // var signer = provider.GetSigner("0x3c44cdddb6a900fa2b585dd299e03d12fa4293bc"); // signer by address

            Console.WriteLine($"Signer address: {await signer.GetAddress()}");
            Console.WriteLine($"Signer balance: {Units.FormatEther(await signer.GetBalance())} ETH");
            Console.WriteLine($"Signer chain id: {await signer.GetChainId()}");
            Console.WriteLine($"Signer tx count: {await signer.GetTransactionCount()}");

            return true;
        }
        //public async Task<bool> SignMessage()
        //{
        // if (_provider is not JsonRpcProvider provider)
        // {
        //     Debug.Log("Provider is not JsonRpcProvider");
        //     return;
        // }
        //
        // var signer = provider.GetSigner(); // default signer at index 0

        // Debug.Log($"Signature string(hello): {await signer.SignMessage("hello")}");
        // Debug.Log($"Signature byte[](hello): {await signer.SignMessage(Encoding.ASCII.GetBytes("hello"))}");

        // var sha3 = new Sha3Keccack();

        // Debug.Log($"Legacy signature string(hello): {await signer._LegacySignMessage(sha3.CalculateHash(Encoding.ASCII.GetBytes("hello")))}");
        // Debug.Log($"Legacy signature byte[](hello): {await signer._LegacySignMessage(Encoding.ASCII.GetBytes("hello"))}");

        //    return true;
        // }
        /// <summary>
        /// SendTransaction
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SendTransaction()
        {
            if (_provider is not JsonRpcProvider provider)
            {
                Console.WriteLine("Provider is not JsonRpcProvider");
                return false;
            }

            var signer = provider.GetSigner(); // default signer at index 0

            var tx = await signer.SendTransaction(new TransactionRequest
            {
                To = await signer.GetAddress(),
                Value = new HexBigInteger(100000)
            });

            Console.WriteLine($"Transaction hash: {tx.Hash}");

            var txReceipt = await tx.Wait();
            Console.WriteLine($"Transaction receipt: {txReceipt.Confirmations}");

            return true;
        }

        /// <summary>
        /// SendContract
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SendContract()
        {
            if (_provider is not JsonRpcProvider provider)
            {
                Console.WriteLine("Provider is not JsonRpcProvider");
                return false;
            }

            var signer = provider.GetSigner(); // default signer at index 0

            var contract = new Contract(NFT_ABI, NFT_ADDRESS, _provider);
            var ret = await contract.Connect(signer).Send("mint");
            Console.WriteLine($"NFT.mint(): {ret}");

            return true;
        }

        /// <summary>
        /// SendContractOverrideGas
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SendContractOverrideGas()
        {
            if (_provider is not JsonRpcProvider provider)
            {
                Console.WriteLine("Provider is not JsonRpcProvider");
                return false;
            }

            var signer = provider.GetSigner(); // default signer at index 0

            var contract = new Contract(NFT_ABI, NFT_ADDRESS, _provider);
            var ret = await contract.Attach("0x000...000").Connect(signer).Send("mint", null, new TransactionRequest
            {
                GasLimit = new HexBigInteger("1000"),
                GasPrice = new HexBigInteger("1000")
            });
            Console.WriteLine($"NFT.mint(): {ret}");

            return true;
        }

        /// <summary>
        /// SubscribeToNewBlockEvent
        /// </summary>
        /// <returns></returns>
        public Task<bool> SubscribeToNewBlockEvent()
        {
            _provider.On("block", (ulong blockNumber) =>
            {
                Console.WriteLine($"New block {blockNumber}");
                return null;
            });

            return Task.FromResult(true);
        }

        /// <summary>
        /// SubscribeToNewChainEvent
        /// </summary>
        /// <returns></returns>
        public Task<bool> SubscribeToNewChainEvent()
        {
            _provider.On("chainChanged", (ulong chainId) =>
            {
                Console.WriteLine($"Chain changed {chainId}");
                return null;
            });

            return Task.FromResult(true);
        }

        /// <summary>
        /// OnApplicationQuit
        /// </summary>
        private void OnApplicationQuit()
        {
            Console.WriteLine("OnApplicationQuit");
            _provider?.RemoveAllListeners();
        }
    }
}