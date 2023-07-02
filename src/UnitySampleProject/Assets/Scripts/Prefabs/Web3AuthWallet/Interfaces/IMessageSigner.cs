// Interface for signing messages
public interface IMessageSigner
{
    string SignMessage(string privateKey, string message);
}

