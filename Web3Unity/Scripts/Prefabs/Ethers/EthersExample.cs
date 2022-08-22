using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Org.BouncyCastle.Crypto.Digests;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.InternalEvents;
using Web3Unity.Scripts.Library.Ethers.Network;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Web3Unity.Scripts.Library.Ethers.Utils;

namespace Web3Unity.Scripts.Prefabs.Ethers
{
    public class EthersExample : MonoBehaviour
    {
        async void Start()
        {
            var chains = await Chains.GetChains();
            Debug.Log($"{chains.Count} chains found");
            
            // var provider = new JsonRpcProvider("https://eth-mainnet.alchemyapi.io/v2/-xZN4CbntuRCIKrOSZgfdxUpV9zG_KtM");
            // var provider = new JsonRpcProvider(); // default url http://localhost:8545
            // var provider = new JsonRpcProvider(chains[1].RPC[3]);
            var provider = new Web3Provider(new WebGLProvider());
            
            var BAYC = "0xBC4CA0EdA7647A8aB7C2061c2E118A18a936f13D";
            
            var network = await provider.GetNetwork();
            Debug.Log($"Network name: {network.Name}");
            Debug.Log($"Network chain id: {network.ChainId}");
            
            var balance = await provider.GetBalance(BAYC);
            Debug.Log($"Contract balance: {balance} wei");
            Debug.Log($"Contract balance: {Units.FormatEther(balance)} ETH");
            
            // var code = await provider.GetCode(BAYC);
            // Debug.Log($"Contract code: {code}");
            
            var slot0 =
                await provider.GetStorageAt(BAYC, new BigInteger(0));
            Debug.Log($"Contract slot 0: {slot0}");
            
            var latestBlock = await provider.GetBlock();
            Debug.Log($"Last block hash: {latestBlock.BlockHash}");
            Debug.Log($"Last block parent hash: {latestBlock.ParentHash}");
            Debug.Log($"Last block number: {latestBlock.Number}");
            Debug.Log($"Last block timestamp: {latestBlock.Timestamp}");
            Debug.Log($"Last block nonce: {latestBlock.Nonce}");
            Debug.Log($"Last block gas limit: {latestBlock.GasLimit}");
            Debug.Log($"Last block gas used: {latestBlock.GasUsed}");
            Debug.Log($"Last block hash: {latestBlock.BlockHash}");
            
            var currentBlockNumber = await provider.GetBlockNumber();
            Debug.Log($"Current block number: {currentBlockNumber}");
            
            var blockByNumber = await provider.GetBlock(new BlockParameter(currentBlockNumber));
            Debug.Log($"Block hash of current block number: {blockByNumber.BlockHash}");
            
            var blockWithTx = await provider.GetBlockWithTransactions(new BlockParameter(currentBlockNumber));
            Debug.Log($"Block hash of first transaction in current block: {blockWithTx.Transactions[0].BlockHash}");
            
            var block15M = await provider.GetBlock(new BlockParameter(15000000));
            if (block15M != null)
            {
                Debug.Log($"Block hash of 15000000: {block15M.BlockHash}");
            }
            else
            {
                Debug.Log($"Block 15000000 not found");
            }
            
            var txCount = await provider.GetTransactionCount("0xaba7161a7fb69c88e16ed9f455ce62b791ee4d03");
            Debug.Log($"Transaction count: {txCount}");
            
            var feeData = await provider.GetFeeData();
            Debug.Log($"GasPrice: {Units.FormatUnits(feeData.GasPrice, "gwei")} gwei");
            Debug.Log($"MaxFeePerGas: {Units.FormatUnits(feeData.MaxFeePerGas, "gwei")} gwei");
            Debug.Log($"MaxPriorityFeePerGas: {Units.FormatUnits(feeData.MaxPriorityFeePerGas, "gwei")} gwei");
            
            var gasPrice = await provider.GetGasPrice();
            Debug.Log($"GasPrice: {gasPrice} wei");
            
            var gwei = Units.FormatUnits(gasPrice, "gwei");
            Debug.Log($"GasPrice: {gwei} gwei");
            
            // Debug.Log(Units.ParseUnits(gwei, "gwei")); // FIXME: this is not working
            
            var receipt =
                await provider.GetTransactionReceipt(
                    "0xad68326264ad8f7b6603dd7350aa9780eb5597228fc04153ddf648eaa624cf60");
            Debug.Log($"Block hash from TX receipt: {receipt.BlockHash}");
            
            var owner = await provider.Call(new TransactionRequest
            {
                To = BAYC,
                Data = "0x8da5cb5b" // owner()
            });
            Debug.Log($"BAYC.onwer(): {owner}");
            
            var ownerOfOne = await provider.Call(new TransactionRequest
            {
                To = BAYC,
                Data = "0x6352211e0000000000000000000000000000000000000000000000000000000000000001" // ownerOf(1)
            });
            
            Debug.Log($"BAYC.onwerOf(1): {ownerOfOne}");
            
            var estimatedGas = await provider.EstimateGas(new TransactionRequest
            {
                To = BAYC,
                Data = "0x6352211e0000000000000000000000000000000000000000000000000000000000000001" // ownerOf(1)
            });
            
            Debug.Log($"Estimated gas for {BAYC}.ownerOf(1): {Units.FormatEther(estimatedGas)} ETH");
            
            // var logs = await provider.GetLogs(new NewFilterInput());
            // Debug.Log($"Number of logs from default filter: {logs.Length}");
            
            var accounts = await provider.ListAccounts();
            foreach (var account in accounts)
            {
                var accountBalance = await provider.GetBalance(account);
                Debug.Log($"{account} balance: {Units.FormatEther(accountBalance)} ETH");
            }
            
            var signer = provider.GetSigner(); // default signer at index 0
            // var signer = provider.GetSigner(1); // signer at index 1
            // var signer = provider.GetSigner("0x3c44cdddb6a900fa2b585dd299e03d12fa4293bc"); // signer by address
            
            Debug.Log($"Signer address: {await signer.GetAddress()}");
            Debug.Log($"Signer balance: {Units.FormatEther(await signer.GetBalance())} ETH");
            Debug.Log($"Signer chain id: {await signer.GetChainId()}");
            Debug.Log($"Signer tx count: {await signer.GetTransactionCount()}");
            
            // Debug.Log($"Signature string(hello): {await signer.SignMessage("hello")}");
            // Debug.Log($"Signature byte[](hello): {await signer.SignMessage(Encoding.ASCII.GetBytes("hello"))}");
            
            var sha3 = new Sha3Keccack();

            // Debug.Log($"Legacy signature string(hello): {await signer._LegacySignMessage(sha3.CalculateHash(Encoding.ASCII.GetBytes("hello")))}");
            // Debug.Log($"Legacy signature byte[](hello): {await signer._LegacySignMessage(Encoding.ASCII.GetBytes("hello"))}");
            
            var tx = await signer.SendTransaction(new TransactionRequest
            {
                To = await signer.GetAddress()
            });
            
            Debug.Log($"Transaction hash: {tx.Hash}");
            
            var txReceipt = await tx.Wait();
            Debug.Log($"Transaction receipt: {txReceipt.BlockNumber}");

            // var signerPriv = Wallet.CreateRandom();
            // Debug.Log($"SignMessage('hello'): {await signerPriv.SignMessage("hello")}");
        }
    }
}