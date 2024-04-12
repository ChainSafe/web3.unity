using System;
using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.SygmaClient.Types;
using ChainSafe.Gaming.Web3;
using Nethereum.ABI;
using Nethereum.Hex.HexConvertors.Extensions;

namespace ChainSafe.Gaming.SygmaClient.DepositDataHandlers
{
    // Struct so we don't allocate memory on the heap
    public class Erc115DepositDataHandler : IDepositDataHandler
    {
        public static readonly Erc115DepositDataHandler Instance = new();

        public byte[] CreateDepositData(Transfer t)
        {
            var transfer = (Transfer<NonFungible>)t;
            BigInteger[] tokenIDs = { BigInteger.Parse(transfer.Details.TokenId) };
            BigInteger[] amounts = { 1 };
            byte[] recipient = transfer.Details.Recipient.HexToByteArray(); // Convert recipient address to byte array
            List<ABIValue> abiValues = new()
            {
                new ABIValue(new DynamicArrayType("uint[]"), tokenIDs),
                new ABIValue(new DynamicArrayType("uint[]"), amounts),
                new ABIValue(new BytesType(), recipient),
                new ABIValue(new BytesType(), Array.Empty<byte>()), // Empty bytes for feeData
            };

            ABIEncode abiEncode = new ABIEncode();
            var depositData = abiEncode.GetABIEncoded(abiValues.ToArray());
            return depositData;
        }
    }
}