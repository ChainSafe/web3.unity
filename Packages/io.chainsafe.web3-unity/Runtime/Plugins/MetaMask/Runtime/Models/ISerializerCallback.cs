namespace MetaMask.Models
{
    /// <summary>
    /// A model or type used as <see cref="MetaMaskEthereumRequest.Parameters"/> field can
    /// implement this interface to be notified when they are about to be serialized into
    /// a request. This can be useful for formatting fields to a standardized type before
    /// serialization occurs.
    /// </summary>
    public interface ISerializerCallback
    {
        /// <summary>
        /// This function is invoked when this instance of the model is about to be
        /// serialized. This function should return a formatted instance of this model
        /// that can be serialized into a request.
        /// </summary>
        object OnSerialize();
    }
}