using System;
using System.Linq;
using ChainSafe.Gaming.Evm.RLP;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Evm.Transactions
{
    /// <summary>
    /// Class to format and parse transactions.
    /// </summary>
    public class Formatter
    {
        /// <summary>
        /// Parses a signed transaction.
        /// </summary>
        /// <param name="signedTx">The signed transaction as a string to be parsed.</param>
        /// <returns>A Transaction object representing the parsed transaction.</returns>
        public Transaction Parse(string signedTx)
        {
            var payload = signedTx.HexToByteArray();

            if (payload[0] > 0x7f)
            {
                return Parse(payload);
            }

            switch (payload[0])
            {
                case 1:
                    return ParseEip2930(payload);
                case 2:
                    return ParseEip1559(payload);
            }

            throw new Exception($"unsupported transaction type: {payload[0]}");
        }

        /// <summary>
        /// Parses a payload of a transaction.
        /// </summary>
        /// <param name="payload">The byte array of the payload to be parsed.</param>
        /// <returns>A Transaction object representing the parsed transaction.</returns>
        private Transaction Parse(byte[] payload)
        {
            var decodedList = RLP.RLP.Decode(payload);
            var decodedElements = (RLPCollection)decodedList;

            if (decodedElements.Count != 6 && decodedElements.Count != 9)
            {
                throw new Exception("invalid raw transaction");
            }

            var tx = new Transaction
            {
                ChainId = new HexBigInteger(0),
                Nonce = new HexBigInteger(decodedElements[0].RLPData.ToBigIntegerFromRLPDecoded()),
                GasPrice = new HexBigInteger(decodedElements[1].RLPData.ToBigIntegerFromRLPDecoded()),
                GasLimit = new HexBigInteger(decodedElements[2].RLPData.ToBigIntegerFromRLPDecoded()),
                To = decodedElements[3].RLPData?.ToHex(true),
                Value = new HexBigInteger(decodedElements[4].RLPData.ToBigIntegerFromRLPDecoded()),
                Data = decodedElements[5].RLPData?.ToHex(true),
            };

            // Legacy unsigned transaction
            if (decodedElements.Count == 6)
            {
                return tx;
            }

            // var signature = RLPDecoder.DecodeSignature(decodedElements, 6);

            // try
            // {
            //     var v = new HexBigInteger(decodedElements[6].RLPData.ToBigIntegerFromRLPDecoded());
            // }
            // catch (Exception e)
            // {
            //     return tx;
            // }

            // tx.r = hexZeroPad(transaction[7], 32);
            // tx.s = hexZeroPad(transaction[8], 32);
            //
            // if (BigNumber.from(tx.r).isZero() && BigNumber.from(tx.s).isZero()) {
            //     // EIP-155 unsigned transaction
            //     tx.chainId = tx.v;
            //     tx.v = 0;
            //
            // } else {
            //     // Signed Transaction
            //
            //     tx.chainId = Math.floor((tx.v - 35) / 2);
            //     if (tx.chainId < 0) { tx.chainId = 0; }
            //
            //     let recoveryParam = tx.v - 27;
            //
            //     const raw = transaction.slice(0, 6);
            //
            //     if (tx.chainId !== 0) {
            //         raw.push(hexlify(tx.chainId));
            //         raw.push("0x");
            //         raw.push("0x");
            //         recoveryParam -= tx.chainId * 2 + 8;
            //     }
            //
            //     const digest = keccak256(RLP.encode(raw));
            //     try {
            //         tx.from = recoverAddress(digest, { r: hexlify(tx.r), s: hexlify(tx.s), recoveryParam: recoveryParam });
            //     } catch (error) { }
            //
            //     tx.hash = keccak256(rawTransaction);
            // }
            //
            // tx.type = null;
            return tx;
        }

        /// <summary>
        /// Parses a payload of a transaction according to the EIP-2930 standard.
        /// </summary>
        /// <param name="payload">The byte array of the payload to be parsed.</param>
        /// <returns>A Transaction object representing the parsed EIP-2930 transaction.</returns>
        private Transaction ParseEip2930(byte[] payload)
        {
            var decodedList = RLP.RLP.Decode(payload.Skip(1).ToArray());
            var decodedElements = (RLPCollection)decodedList;

            if (decodedElements.Count != 8 && decodedElements.Count != 11)
            {
                throw new Exception("invalid component count for transaction type: 1");
            }

            var tx = new Transaction
            {
                Type = new HexBigInteger(1),
                ChainId = new HexBigInteger(decodedElements[0].RLPData.ToBigIntegerFromRLPDecoded()),
                Nonce = new HexBigInteger(decodedElements[1].RLPData.ToBigIntegerFromRLPDecoded()),
                GasPrice = new HexBigInteger(decodedElements[2].RLPData.ToBigIntegerFromRLPDecoded()),
                GasLimit = new HexBigInteger(decodedElements[3].RLPData.ToBigIntegerFromRLPDecoded()),
                To = decodedElements[4].RLPData?.ToHex(true),
                Value = new HexBigInteger(decodedElements[5].RLPData.ToBigIntegerFromRLPDecoded()),
                Data = decodedElements[6].RLPData?.ToHex(true),

                // AccessList = new List<AccessList>(), // TODO: parse this
            };

            // Unsigned EIP-2930 Transaction
            if (decodedElements.Count == 8)
            {
                return tx;
            }

            // tx.Hash = keccak256(payload);
            // _parseEipSignature(tx, decodedElement.Skip(8).ToArray(), _serializeEip2930);
            return tx;
        }

        private Transaction ParseEip1559(byte[] payload)
        {
            var decodedList = RLP.RLP.Decode(payload.Skip(1).ToArray());
            var decodedElements = (RLPCollection)decodedList;

            if (decodedElements.Count != 9 && decodedElements.Count != 12)
            {
                throw new Exception("invalid component count for transaction type: 2");
            }

            var tx = new Transaction
            {
                Type = new HexBigInteger(2),
                ChainId = new HexBigInteger(decodedElements[0].RLPData.ToBigIntegerFromRLPDecoded()),
                Nonce = new HexBigInteger(decodedElements[1].RLPData.ToBigIntegerFromRLPDecoded()),
                MaxPriorityFeePerGas = new HexBigInteger(decodedElements[2].RLPData.ToBigIntegerFromRLPDecoded()),
                MaxFeePerGas = new HexBigInteger(decodedElements[3].RLPData.ToBigIntegerFromRLPDecoded()),
                GasLimit = new HexBigInteger(decodedElements[4].RLPData.ToBigIntegerFromRLPDecoded()),
                To = decodedElements[5].RLPData?.ToHex(true),
                Value = new HexBigInteger(decodedElements[6].RLPData.ToBigIntegerFromRLPDecoded()),
                Data = decodedElements[7].RLPData?.ToHex(true),

                // AccessList = new List<AccessList>(), // TODO: parse this
            };

            // Unsigned EIP-1559 Transaction
            if (decodedElements.Count == 9)
            {
                return tx;
            }

            // tx.Hash = keccak256(payload);
            // _parseEipSignature(tx, decodedElement.Skip(8).ToArray(), _serializeEip2930);
            return tx;
        }
    }
}