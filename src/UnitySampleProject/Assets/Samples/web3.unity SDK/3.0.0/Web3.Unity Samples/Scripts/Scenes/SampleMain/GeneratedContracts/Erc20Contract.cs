using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using ChainSafe.Gaming.RPC.Events;
using ChainSafe.Gaming.UnityPackage;


namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public partial class Erc20Contract : ICustomContract
    {
        public string Address => OriginalContract.Address;

        public string ABI =>
            "[   {     \"inputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"constructor\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"spender\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"allowance\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"needed\",         \"type\": \"uint256\"       }     ],     \"name\": \"ERC20InsufficientAllowance\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"balance\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"needed\",         \"type\": \"uint256\"       }     ],     \"name\": \"ERC20InsufficientBalance\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"approver\",         \"type\": \"address\"       }     ],     \"name\": \"ERC20InvalidApprover\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"receiver\",         \"type\": \"address\"       }     ],     \"name\": \"ERC20InvalidReceiver\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"sender\",         \"type\": \"address\"       }     ],     \"name\": \"ERC20InvalidSender\",     \"type\": \"error\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"spender\",         \"type\": \"address\"       }     ],     \"name\": \"ERC20InvalidSpender\",     \"type\": \"error\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"spender\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"value\",         \"type\": \"uint256\"       }     ],     \"name\": \"Approval\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"value\",         \"type\": \"uint256\"       }     ],     \"name\": \"Transfer\",     \"type\": \"event\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"spender\",         \"type\": \"address\"       }     ],     \"name\": \"allowance\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"spender\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"value\",         \"type\": \"uint256\"       }     ],     \"name\": \"approve\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"account\",         \"type\": \"address\"       }     ],     \"name\": \"balanceOf\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [      ],     \"name\": \"decimals\",     \"outputs\": [       {         \"internalType\": \"uint8\",         \"name\": \"\",         \"type\": \"uint8\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"_to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"_amount\",         \"type\": \"uint256\"       }     ],     \"name\": \"mint\",     \"outputs\": [      ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [      ],     \"name\": \"name\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [      ],     \"name\": \"symbol\",     \"outputs\": [       {         \"internalType\": \"string\",         \"name\": \"\",         \"type\": \"string\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [      ],     \"name\": \"totalSupply\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"value\",         \"type\": \"uint256\"       }     ],     \"name\": \"transfer\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"from\",         \"type\": \"address\"       },       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"value\",         \"type\": \"uint256\"       }     ],     \"name\": \"transferFrom\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"\",         \"type\": \"bool\"       }     ],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   } ]";

        public string ContractAddress { get; set; }

        public IEventManager EventManager { get; set; }

        public Contract OriginalContract { get; set; }

        public bool Subscribed { get; set; }


        #region Methods

        public async Task<BigInteger> Allowance(string owner, string spender)
        {
            var response = await OriginalContract.Call<BigInteger>("allowance", new object[]
            {
                owner, spender
            });

            return response;
        }


        public async Task<bool> Approve(string spender, BigInteger value)
        {
            var response = await OriginalContract.Send<bool>("approve", new object[]
            {
                spender, value
            });

            return response;
        }

        public async Task<(bool, TransactionReceipt receipt)> ApproveWithReceipt(string spender, BigInteger value)
        {
            var response = await OriginalContract.SendWithReceipt<bool>("approve", new object[]
            {
                spender, value
            });

            return (response.response, response.receipt);
        }

        public async Task<BigInteger> BalanceOf(string account)
        {
            var response = await OriginalContract.Call<BigInteger>("balanceOf", new object[]
            {
                account
            });

            return response;
        }


        public async Task<BigInteger> Decimals()
        {
            var response = await OriginalContract.Call<BigInteger>("decimals", new object[]
            {
            });

            return response;
        }


        public async Task Mint(string _to, BigInteger _amount)
        {
            var response = await OriginalContract.Send("mint", new object[]
            {
                _to, _amount
            });
        }

        public async Task<TransactionReceipt> MintWithReceipt(string _to, BigInteger _amount)
        {
            var response = await OriginalContract.SendWithReceipt("mint", new object[]
            {
                _to, _amount
            });

            return response.receipt;
        }

        public async Task<string> Name()
        {
            var response = await OriginalContract.Call<string>("name", new object[]
            {
            });

            return response;
        }


        public async Task<string> Symbol()
        {
            var response = await OriginalContract.Call<string>("symbol", new object[]
            {
            });

            return response;
        }


        public async Task<BigInteger> TotalSupply()
        {
            var response = await OriginalContract.Call<BigInteger>("totalSupply", new object[]
            {
            });

            return response;
        }


        public async Task<bool> Transfer(string to, BigInteger value)
        {
            var response = await OriginalContract.Send<bool>("transfer", new object[]
            {
                to, value
            });

            return response;
        }

        public async Task<(bool, TransactionReceipt receipt)> TransferWithReceipt(string to, BigInteger value)
        {
            var response = await OriginalContract.SendWithReceipt<bool>("transfer", new object[]
            {
                to, value
            });

            return (response.response, response.receipt);
        }

        public async Task<bool> TransferFrom(string from, string to, BigInteger value)
        {
            var response = await OriginalContract.Send<bool>("transferFrom", new object[]
            {
                from, to, value
            });

            return response;
        }

        public async Task<(bool, TransactionReceipt receipt)> TransferFromWithReceipt(string from, string to,
            BigInteger value)
        {
            var response = await OriginalContract.SendWithReceipt<bool>("transferFrom", new object[]
            {
                from, to, value
            });

            return (response.response, response.receipt);
        }

        #endregion


        #region Event Classes

        public partial class ApprovalEventDTO : ApprovalEventDTOBase
        {
        }

        [Event("Approval")]
        public class ApprovalEventDTOBase : IEventDTO
        {
            [Parameter("address", "owner", 0, true)]
            public virtual string Owner { get; set; }

            [Parameter("address", "spender", 1, true)]
            public virtual string Spender { get; set; }

            [Parameter("uint256", "value", 2, false)]
            public virtual BigInteger Value { get; set; }
        }

        public event Action<ApprovalEventDTO> OnApproval;

        private void Approval(ApprovalEventDTO approval)
        {
            OnApproval?.Invoke(approval);
        }

        public partial class TransferEventDTO : TransferEventDTOBase
        {

        }

        [Event("Transfer")]
        public class TransferEventDTOBase : IEventDTO
        {
            [Parameter("address", "from", 0, true)]
            public virtual string From { get; set; }

            [Parameter("address", "to", 1, true)] public virtual string To { get; set; }

            [Parameter("uint256", "value", 2, false)]
            public virtual BigInteger Value { get; set; }

            public override string ToString()
            {
                return $"{nameof(From)}: {From}, {nameof(To)}: {To}, {nameof(Value)}: {Value}";
            }
        }

        public event Action<TransferEventDTO> OnTransfer;

        private void Transfer(TransferEventDTO transfer)
        {
            Debug.Log("Transfer happened " + transfer.Value);
            OnTransfer?.Invoke(transfer);
        }

        #endregion

        #region Interface Implemented Methods

        public async ValueTask DisposeAsync()
        {
            if (!Subscribed)
                return;


            Subscribed = false;
            try
            {
                if (EventManager == null)
                    return;
                await EventManager.Unsubscribe<ApprovalEventDTO>(Approval, ContractAddress);
                OnApproval = null;
                await EventManager.Unsubscribe<TransferEventDTO>(Transfer, ContractAddress);
                OnTransfer = null;
            }
            catch (Exception e)
            {
                Debug.LogError("Caught an exception whilst unsubscribing from events\n" + e.Message);
            }
        }

        public async ValueTask InitAsync()
        {
            if (Subscribed)
                return;
            Subscribed = true;

            try
            {
                if (EventManager == null)
                    return;
                await EventManager.Subscribe<ApprovalEventDTO>(Approval, ContractAddress);
                await EventManager.Subscribe<TransferEventDTO>(Transfer, ContractAddress);
            }
            catch (Exception e)
            {
                Debug.LogError(
                    "Caught an exception whilst subscribing to events. Subscribing to events will not work in this session\n" +
                    e.Message);
            }
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public IContract Attach(string address)
        {
            return OriginalContract.Attach(address);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.Call(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public object[] Decode(string method, string output)
        {
            return OriginalContract.Decode(method, output);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<object[]> Send(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.Send(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(string method,
            object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.SendWithReceipt(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<HexBigInteger> EstimateGas(string method, object[] parameters, TransactionRequest overwrite = null)
        {
            return OriginalContract.EstimateGas(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public string Calldata(string method, object[] parameters = null)
        {
            return OriginalContract.Calldata(method, parameters);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters,
            bool isReadCall = false, TransactionRequest overwrite = null)
        {
            return OriginalContract.PrepareTransactionRequest(method, parameters, isReadCall, overwrite);
        }

        #endregion
    }
}