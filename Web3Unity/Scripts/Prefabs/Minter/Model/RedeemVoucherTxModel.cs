using System;

namespace Models
{
    public class RedeemVoucherTxModel
    {
    [Serializable]
    public class Connection
    {
        public string chain { get; set; }
        public string network { get; set; }
    }
    [Serializable]
    public class Response
    {
        public string hashedUnsignedTx { get; set; }
        public Connection connection { get; set; }
        public string tokenId { get; set; }
        public Tx tx { get; set; }
    }
    [Serializable]
    public class Root
    {
        public Response response { get; set; }
    }
    [Serializable]
    public class Tx
    {
        public string account { get; set; }
        public string to { get; set; }
        public int value { get; set; }
        public string data { get; set; }
        public string gasPrice { get; set; }
        public string gasLimit { get; set; }
    }
    }
}