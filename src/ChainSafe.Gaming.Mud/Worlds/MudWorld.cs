using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Mud.Storages;
using ChainSafe.Gaming.Mud.Systems;
using ChainSafe.Gaming.Mud.Tables;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Mud.Worlds
{
    /// <summary>
    /// Represents a client for a MUD world.
    /// </summary>
    public class MudWorld : IContract
    {
        private readonly MudWorldSystems systems;
        private readonly MudWorldTables tables;

        private readonly Contract contract;
        private readonly string? defaultNamespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="MudWorld"/> class.
        /// </summary>
        public MudWorld(IMudWorldConfig config, IMudStorage storage, IContractBuilder contractBuilder, IMainThreadRunner mainThreadRunner)
        {
            contract = contractBuilder.Build(config.ContractAbi, config.ContractAddress);
            defaultNamespace = config.DefaultNamespace;

            tables = new MudWorldTables(config.TableSchemas, storage, mainThreadRunner);
            systems = new MudWorldSystems(contract);
        }

        /// <summary>
        /// Retrieve MudTable for the specified table name in the default namespace.
        /// </summary>
        /// <returns>The MudTable for the specified table name.</returns>
        public MudTable GetTable(string tableName)
        {
            AssertDefaultNamespaceAvailable();
            return tables.GetTable(defaultNamespace!, tableName);
        }

        /// <summary>
        /// Retrieve MudTable for the specified table name and namespace.
        /// </summary>
        /// <returns>The MudTable for the specified table name and namespace.</returns>
        public MudTable GetTable(string tableName, string @namespace)
        {
            return tables.GetTable(@namespace, tableName);
        }

        /// <summary>
        /// Retrieve client for Mud Systems for the default namespace.
        /// </summary>
        /// <remarks>Use this to call system functions.</remarks>
        /// <returns>The MudSystems for the default namespace.</returns>
        public MudSystems GetSystems()
        {
            AssertDefaultNamespaceAvailable();
            return systems.GetSystemsForNamespace(defaultNamespace!);
        }

        /// <summary>
        /// Retrieve client for Mud Systems for the specified namespace.
        /// </summary>
        /// <remarks>Use this to call system functions.</remarks>
        /// <returns>The MudSystems for the specified namespace.</returns>
        public MudSystems GetSystems(string @namespace)
        {
            return systems.GetSystemsForNamespace(@namespace);
        }

        private void AssertDefaultNamespaceAvailable()
        {
            if (defaultNamespace is null)
            {
                throw new MudException("Default Namespace was not provided in World Config.");
            }
        }

#pragma warning disable SA1124
        #region IContract delegation
#pragma warning restore SA1124
#pragma warning disable SA1201
        public string Address => contract.Address;
#pragma warning restore SA1201

        public IContract Attach(string address)
        {
            return contract.Attach(address);
        }

        public Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return contract.Call(method, parameters, overwrite);
        }

        public object[] Decode(string method, string output)
        {
            return contract.Decode(method, output);
        }

        public Task<object[]> Send(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return contract.Send(method, parameters, overwrite);
        }

        public Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return contract.SendWithReceipt(method, parameters, overwrite);
        }

        public Task<HexBigInteger> EstimateGas(string method, object[] parameters, TransactionRequest overwrite = null)
        {
            return contract.EstimateGas(method, parameters, overwrite);
        }

        public string Calldata(string method, object[] parameters = null)
        {
            return contract.Calldata(method, parameters);
        }

        public Task<TransactionRequest> PrepareTransactionRequest(
            string method,
            object[] parameters,
            bool isReadCall = false,
            TransactionRequest overwrite = null)
        {
            return contract.PrepareTransactionRequest(method, parameters, isReadCall, overwrite);
        }

        #endregion
    }
}