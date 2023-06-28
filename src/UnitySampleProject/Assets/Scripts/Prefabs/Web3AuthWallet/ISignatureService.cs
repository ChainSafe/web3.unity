public interface ISignatureService
{
    string SignTransaction(string privateKey, string transaction, string chainId);
    
    // used to sign a message with a users private key stored in memory
    public string SignMsgW3A(string _privateKey, string _message);
}