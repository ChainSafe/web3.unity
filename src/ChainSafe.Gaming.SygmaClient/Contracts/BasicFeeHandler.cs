using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.Builders;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.SygmaClient.Dto;
using ChainSafe.Gaming.SygmaClient.Types;
using Nethereum.ABI;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;

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
            this.contract = cb.Build(FeeHandlerAbi, address);
        }

        public async Task<EvmFee> CalculateBasicFee(
            string sender,
            string reciever,
            uint fromDomainID,
            uint destinationDomainID,
            HexBigInteger resourceID,
            EvmFee feeData)
        {
            var data = CreateERC1155DepositData("1", reciever);
            var result = await this.contract.Call(MethodCalculateFee, new object[] { sender, fromDomainID, destinationDomainID, resourceID.ToHexByteArray(), data, feeData.FeeData.HexToByteArray() });

// Your original code for creating the fee object
            var fee = new EvmFee(this.address, FeeHandlerType.Basic)
            {
                Fee = BigInteger.Parse(result[0].ToString()),
                FeeData = result[1] as string,
            };

            return fee;
        }

        private byte[] CreateERC1155DepositData(string tokenId, string reciever)
        {
            // Your data to encode
            BigInteger[] tokenIDs = { BigInteger.Parse(tokenId) };
            BigInteger[] amounts = { 1 };
            byte[] recipient = reciever.HexToByteArray(); // Convert recipient address to byte array
            List<ABIValue> abivalues = new();
            abivalues.Add(new ABIValue(new DynamicArrayType("uint[]"), tokenIDs));
            abivalues.Add(new ABIValue(new DynamicArrayType("uint[]"), amounts));
            abivalues.Add(new ABIValue(new BytesType(), recipient));
            abivalues.Add(new ABIValue(new BytesType(), Array.Empty<byte>()));

            ABIEncode abiEncode = new ABIEncode();
            var depositData = abiEncode.GetABIEncoded(abivalues.ToArray());
            return depositData;
        }
    }
}