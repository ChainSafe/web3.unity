using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.SygmaClient.Types;

namespace ChainSafe.Gaming.SygmaClient.Contracts
{
    public class FeeHandlerRouter : IFeeHandlerRouter
    {
        private const string MethodDomainResourceIDToFeeHandlerAddress = "_domainResourceIDToFeeHandlerAddress";
        private const string FeeHandlerAbi =
            "[ {\"inputs\": [],\"name\": \"DEFAULT_ADMIN_ROLE\",\"outputs\": [ { \"internalType\": \"bytes32\", \"name\": \"\", \"type\": \"bytes32\"  } ],\"stateMutability\": \"view\",\"type\": \"function\" }, {\"inputs\": [],\"name\": \"_bridgeAddress\",\"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\"  } ],\"stateMutability\": \"view\",\"type\": \"function\" }, {\"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\"     },     { \"internalType\": \"bytes32\", \"name\": \"\", \"type\": \"bytes32\"  } ],\"name\": \"_domainResourceIDToFeeHandlerAddress\",\"outputs\": [ { \"internalType\": \"contract IFeeHandler\", \"name\": \"\", \"type\": \"address\"  } ],\"stateMutability\": \"view\",\"type\": \"function\" }, {\"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\"  } ],\"name\": \"_whitelist\",\"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\"  } ],\"stateMutability\": \"view\",\"type\": \"function\" }, {\"inputs\": [ { \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\"  } ],\"name\": \"getRoleAdmin\",\"outputs\": [ { \"internalType\": \"bytes32\", \"name\": \"\", \"type\": \"bytes32\"  } ],\"stateMutability\": \"view\",\"type\": \"function\" }, {\"inputs\": [ { \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\"     },     { \"internalType\": \"uint256\", \"name\": \"index\", \"type\": \"uint256\"  } ],\"name\": \"getRoleMember\",\"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\"  } ],\"stateMutability\": \"view\",\"type\": \"function\" }, {\"inputs\": [ { \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\"  } ],\"name\": \"getRoleMemberCount\",\"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\"  } ],\"stateMutability\": \"view\",\"type\": \"function\" }, {\"inputs\": [ { \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\"     },     { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\"  } ],\"name\": \"getRoleMemberIndex\",\"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\"  } ],\"stateMutability\": \"view\",\"type\": \"function\" }, {\"inputs\": [ { \"internalType\": \"bytes32\", \"name\": \"role\", \"type\": \"bytes32\"     },     { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\"  } ],\"name\": \"hasRole\",\"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\"  } ],\"stateMutability\": \"view\",\"type\": \"function\" }, {\"inputs\": [ { \"internalType\": \"address\", \"name\": \"sender\", \"type\": \"address\"     },     { \"internalType\": \"uint8\", \"name\": \"fromDomainID\", \"type\": \"uint8\"     },     { \"internalType\": \"uint8\", \"name\": \"destinationDomainID\", \"type\": \"uint8\"     },     { \"internalType\": \"bytes32\", \"name\": \"resourceID\", \"type\": \"bytes32\"     },     { \"internalType\": \"bytes\", \"name\": \"depositData\", \"type\": \"bytes\"     },     { \"internalType\": \"bytes\", \"name\": \"feeData\", \"type\": \"bytes\"  } ],\"name\": \"calculateFee\",\"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"fee\", \"type\": \"uint256\"     },     { \"internalType\": \"address\", \"name\": \"tokenAddress\", \"type\": \"address\"  } ],\"stateMutability\": \"view\",\"type\": \"function\" } ]";

        private readonly Contract contract;

        public FeeHandlerRouter(IContractBuilder cb, string address)
        {
            this.contract = cb.Build(FeeHandlerAbi, address);
        }

        public Task<string> DomainResourceIDToFeeHandlerAddress(
            uint domainID,
            string resourceID)
        {
            var result = this.contract.Call(MethodDomainResourceIDToFeeHandlerAddress, new object[] { domainID, resourceID });
            return Task.FromResult(result.ToString());
        }
    }
}