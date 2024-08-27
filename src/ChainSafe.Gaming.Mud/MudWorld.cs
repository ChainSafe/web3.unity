using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.GasFees;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Mud;
using Nethereum.Mud.Contracts.World;
using Nethereum.Web3;

namespace ChainSafe.Gaming.Mud
{
    // todo add event subscription
    public class MudWorld : IMudWorld, IContract
    {
        private readonly IContract contract;

        public MudWorld(IWeb3 nethWeb3, IContract contract)
        {
            this.contract = contract;
            WorldService = new WorldService(nethWeb3, contract.Address);
        }

        string IContract.Address => contract.Address;

        /// <summary>
        /// A Nethereum World Service. Use this if you need more control over the World.
        /// </summary>
        public WorldService WorldService { get; }

        public async Task<TValue> Query<TRecord, TValue>()
            where TRecord : TableRecordSingleton<TValue>, new()
            where TValue : class, new()
        {
            return (await WorldService.GetRecordTableQueryAsync<TRecord, TValue>()).Values;
        }

        public async Task<TValue> Query<TRecord, TKey, TValue>(TKey key)
            where TRecord : TableRecord<TKey, TValue>, new()
            where TKey : class, new()
            where TValue : class, new()
        {
            var record = new TRecord { Keys = key };
            return (await WorldService.GetRecordTableQueryAsync<TRecord, TKey, TValue>(record)).Values;
        }

        IContract IContract.Attach(string address)
        {
            return contract.Attach(address);
        }

        Task<object[]> IContract.Call(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return contract.Call(method, parameters, overwrite);
        }

        object[] IContract.Decode(string method, string output)
        {
            return contract.Decode(method, output);
        }

        Task<object[]> IContract.Send(string method, object[] parameters = null, TransactionRequest overwrite = null, IGasFeeModifier gasFeeModifier = null)
        {
            return contract.Send(method, parameters, overwrite, gasFeeModifier);
        }

        Task<(object[] response, TransactionReceipt receipt)> IContract.SendWithReceipt(string method, object[] parameters = null, TransactionRequest overwrite = null, IGasFeeModifier gasFeeModifier = null)
        {
            return contract.SendWithReceipt(method, parameters, overwrite, gasFeeModifier);
        }

        Task<HexBigInteger> IContract.EstimateGas(string method, object[] parameters, TransactionRequest overwrite = null)
        {
            return contract.EstimateGas(method, parameters, overwrite);
        }

        string IContract.Calldata(string method, object[] parameters = null)
        {
            return contract.Calldata(method, parameters);
        }

        public Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters, bool isReadCall = false, TransactionRequest overwrite = null)
        {
            return contract.PrepareTransactionRequest(method, parameters, isReadCall, overwrite);
        }
    }
}