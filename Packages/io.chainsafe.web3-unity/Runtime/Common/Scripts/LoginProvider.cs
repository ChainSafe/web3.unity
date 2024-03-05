using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Common;
using UnityEngine;

namespace Scenes
{
    /// <summary>
    /// A concrete implementation of <see cref="ILoginProvider"/>.
    /// </summary>
    public abstract class LoginProvider : MonoBehaviour, ILoginProvider
    {
        [SerializeField] private string gelatoApiKey = "";

        [SerializeField] private ErrorPopup errorPopup;

        public string GelatoApiKey => gelatoApiKey;
        public IWeb3BuilderServiceAdapter[] Web3BuilderServiceAdapters { get; private set; }
        public IWeb3InitializedHandler[] Web3InitializedHandlers { get; private set; }

        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes Login providers.
        /// </summary>
        protected virtual void Initialize()
        {
            Web3BuilderServiceAdapters = GetComponents<IWeb3BuilderServiceAdapter>();

            Web3InitializedHandlers = GetComponents<IWeb3InitializedHandler>();
        }

        /// <summary>
        /// Try to Login and displays error and throws exception on a failed attempt.
        /// </summary>
        public async Task TryLogin()
        {
            try
            {
                await (this as ILoginProvider).Login();
            }
            catch (Exception e)
            {
                errorPopup.ShowError($"Login failed, please try again\n{e.Message} (see console for more details)");

                throw;
            }
        }
    }
}