using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.SygmaClient.DepositDataHandlers;
using ChainSafe.Gaming.SygmaClient.Dto;
using ChainSafe.Gaming.SygmaClient.Types;
using Nethereum.Hex.HexConvertors.Extensions;

namespace ChainSafe.Gaming.SygmaClient.Contracts
{
    public class BasicFeeHandler : IBasicHandler
    {
        private const string MethodCalculateFee = "calculateFee";
        private const string FeeHandlerAbi =
            "[ { \"inputs\": [  { \"internalType\": \"address\", \"name\": \"bridgeAddress\", \"type\": \"address\"  },  { \"internalType\": \"address\", \"name\": \"feeHandlerRouterAddress\", \"type\": \"address\"  }],\"stateMutability\": \"nonpayable\",\"type\": \"constructor\"\n  },\n  {\"inputs\": [  { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\"  },  { \"internalType\": \"bytes32\", \"name\": \"\", \"type\": \"bytes32\"  }],\"name\": \"_domainResourceIDToFee\",\"outputs\": [  { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\"  }],\"stateMutability\": \"view\",\"type\": \"function\"\n  },\n  {\"inputs\": [],\"name\": \"_feeHandlerRouterAddress\",\"outputs\": [  { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\"  }],\"stateMutability\": \"view\",\"type\": \"function\"\n  },\n  {\"inputs\": [  { \"internalType\": \"address\", \"name\": \"sender\", \"type\": \"address\"  },  { \"internalType\": \"uint8\", \"name\": \"fromDomainID\", \"type\": \"uint8\"  },  { \"internalType\": \"uint8\", \"name\": \"destinationDomainID\", \"type\": \"uint8\"  },  { \"internalType\": \"bytes32\", \"name\": \"resourceID\", \"type\": \"bytes32\"  },  { \"internalType\": \"bytes\", \"name\": \"depositData\", \"type\": \"bytes\"  },  { \"internalType\": \"bytes\", \"name\": \"feeData\", \"type\": \"bytes\"  }],\"name\": \"calculateFee\",\"outputs\": [  { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\"  },  { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\"  }],\"stateMutability\": \"view\",\"type\": \"function\"\n  },\n  {\"inputs\": [  { \"internalType\": \"address payable[]\", \"name\": \"addrs\", \"type\": \"address[]\"  },  { \"internalType\": \"uint256[]\", \"name\": \"amounts\", \"type\": \"uint256[]\"  }],\"name\": \"transferFee\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\" } ]";

        private readonly Contract contract;
        private readonly string address;

        public BasicFeeHandler(IContractBuilder cb, string address)
        {
            this.address = address;
            contract = cb.Build(FeeHandlerAbi, address);
        }

        public async Task<EvmFee> CalculateBasicFee(Transfer transfer, EvmFee feeData)
        {
            var handler = DepositDataFactory.Handler(transfer.Resource.Type);
            var depositData = handler.CreateDepositData(transfer);

            var result = await contract.Call(MethodCalculateFee, new object[] { transfer.Sender, transfer.From.Id, transfer.To.Id, transfer.Resource.ResourceId.HexToByteArray(), depositData, feeData.FeeData.HexToByteArray() });

            var fee = new EvmFee(address, FeeHandlerType.Basic)
            {
                Fee = BigInteger.Parse(result[0].ToString()),
                FeeData = result[1] as string,
            };

            return fee;
        }
    }
}