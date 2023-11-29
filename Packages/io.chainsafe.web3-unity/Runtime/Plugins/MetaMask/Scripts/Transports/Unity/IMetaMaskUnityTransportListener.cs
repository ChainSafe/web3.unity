using System;

using MetaMask.Models;

namespace MetaMask.Transports.Unity
{

    public interface IMetaMaskUnityTransportListener
    {

        /// <summary>Called when the MetaMask client wants to connect to the application.</summary>
        /// <param name="url">The URL to connect to.</param>
        void OnMetaMaskConnectRequest(string universalLink, string deepLink);

        /// <summary>Handles a MetaMask request.</summary>
        /// <param name="id">The request ID.</param>
        /// <param name="request">The request.</param>
        void OnMetaMaskRequest(string id, MetaMaskEthereumRequest request);

        /// <summary>Called when MetaMask fails to connect.</summary>
        /// <param name="error">The error that occurred.</param>
        void OnMetaMaskFailure(Exception error);

        /// <summary>Called when the MetaMask login was successful.</summary>
        void OnMetaMaskSuccess();

        /// <summary>
        /// Called when the MetaMask connection requires an OTP code
        /// </summary>
        /// <param name="otp">The OTP code to show</param>
        void OnMetaMaskOTP(int otp);

        void OnMetaMaskDisconnected();

    }

}