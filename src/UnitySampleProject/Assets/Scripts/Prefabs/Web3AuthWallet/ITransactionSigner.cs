// Interface for signing transactions
public interface ITransactionSigner
{
    string SignTransaction(string privateKey, string transaction);
}
