namespace evm.net.Models
{
    public struct CallOptions
    {
        public string Gas { get; set; }
        
        public string GasPrice { get; set; }
        
        public string Nonce { get; set; }
        
        public string BlockTag { get; set; }
        
        public string Value { get; set; }
    }
}