using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.Evm.Utils;
using ChainSafe.Gaming.SygmaClient.Types;
using Nethereum.Hex.HexConvertors.Extensions;

namespace ChainSafe.Gaming.SygmaClient.DepositDataHandlers
{
    public class Erc721DepositDataHandler : IDepositDataHandler
    {
        public static readonly Erc721DepositDataHandler Instance = new();

        public byte[] CreateDepositData(Transfer t)
        {
            var transfer = (Transfer<NonFungible>)t;

            // Convert tokenId to a BigInteger and ensure it is a positive value.
            BigInteger tokenBigInt = BigInteger.Parse(transfer.Details.TokenId);

            // Ensure the tokenId is represented in a 32-byte array, left-padded with zeros.
            byte[] tokenIdBytes = tokenBigInt.ToByteArray().Reverse().ToArray(); // Reverse to ensure little-endian to big-endian conversion if necessary.
            if (tokenIdBytes.Length < 32)
            {
                tokenIdBytes = tokenIdBytes.Concat(new byte[32 - tokenIdBytes.Length]).ToArray(); // Left-pad with zeros if necessary.
            }
            else if (tokenIdBytes.Length > 32)
            {
                throw new ArgumentException("Token ID is too large.");
            }

            // Convert recipient string to byte array.
            byte[] recipientBytes = Units.ConvertHexStringToByteArray(transfer.Details.Recipient);

            // Encode the length of the recipient byte array as a 32-byte array.
            BigInteger recipientLengthBigInt = new BigInteger(recipientBytes.Length);
            byte[] recipientLengthBytes = recipientLengthBigInt.ToByteArray().Reverse().ToArray();
            if (recipientLengthBytes.Length < 32)
            {
                recipientLengthBytes = recipientLengthBytes.Concat(new byte[32 - recipientLengthBytes.Length]).ToArray();
            }

            // Concatenate the tokenIdBytes, recipientLengthBytes, and recipientBytes.
            List<byte> data = new List<byte>();
            data.AddRange(tokenIdBytes);
            data.AddRange(recipientLengthBytes);
            data.AddRange(recipientBytes);

            // Convert the resulting byte array to a hexadecimal string, ensuring it is prefixed with "0x".
            return ("0x" + BitConverter.ToString(data.ToArray()).Replace("-", string.Empty).ToLower()).HexToByteArray();
        }
    }
}