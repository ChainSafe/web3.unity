using System.Numerics;
using ChainSafe.Gaming.SygmaClient.Dto;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public class Transfer
    {
        protected Transfer(Domain to, Domain from, string sender)
        {
            To = to;
            From = from;
            Sender = sender;
        }

        public Domain To { get; }

        public Domain From { get; }

        public BaseResources Resource { get; set; }

        public string Sender { get; }
    }

    public class Transfer<T> : Transfer
        where T : TransferType
    {
        public Transfer(Domain to, Domain from, string sender)
            : base(to, from, sender)
        {
        }

        public T Details { get; set; }
    }

    public class TransferType
    {
        protected TransferType(string recipient)
        {
            Recipient = recipient;
        }

        public string Recipient { get; }
    }

    public class Fungible : TransferType
    {
        public Fungible(string recipient, HexBigInteger amount)
            : base(recipient)
        {
            Amount = amount ?? new HexBigInteger("0x0");
        }

        public HexBigInteger Amount { get; }
    }

    public class NonFungible : Fungible
    {
        public NonFungible(string recipient, string tokenId, HexBigInteger amount = null)
            : base(recipient, amount)
        {
            TokenId = tokenId;
        }

        public string TokenId { get; }
    }

    public class Erc1155Deposit
    {
        [Parameter("unit256[]", 1)]
        public BigInteger[] TokenIds { get; set; }

        [Parameter("unit256[]", 2)]
        public BigInteger[] Amounts { get; set; }

        [Parameter("bytes", 3)]
        public byte[] DestinationRecipientAddress { get; set; }
    }
}