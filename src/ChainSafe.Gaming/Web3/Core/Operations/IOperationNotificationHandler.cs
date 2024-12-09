namespace ChainSafe.Gaming.Web3.Core.Operations
{
    /// <summary>
    /// Represents an object capable of handling incoming requests to show notifications to user.
    /// </summary>
    public interface IOperationNotificationHandler
    {
        void OnNotificationsAvailable();

        void OnNotificationsOver();

        void SetCurrentOperation(string message);
    }
}