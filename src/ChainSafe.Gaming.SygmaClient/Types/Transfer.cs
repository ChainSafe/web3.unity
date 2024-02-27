using ChainSafe.Gaming.SygmaClient.Dto;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public class Transfer<T>
        where T : TransferType
    {
        public Transfer(Domain to, Domain from, string sender)
        {
            To = to;
            From = from;
            Sender = sender;
        }

        public T Details { get; set; }

        public Domain To { get; }

        public Domain From { get; }

        public BaseResources Resource { get; set; }

        public string Sender { get; }
    }

    public class TransferType
    {
        public TransferType(string recipient)
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
            Amount = amount;
        }

        public HexBigInteger Amount { get; }
    }

    public class NonFungible : TransferType
    {
        public NonFungible(string recipient, string tokenId)
            : base(recipient)
        {
            TokenId = tokenId;
        }

        public string TokenId { get; }
    }
}