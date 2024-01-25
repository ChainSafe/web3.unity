using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Common;
using UnityEngine;

namespace Scenes
{
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

        protected virtual void Initialize()
        {
            Web3BuilderServiceAdapters = GetComponents<IWeb3BuilderServiceAdapter>();
            
            Web3InitializedHandlers = GetComponents<IWeb3InitializedHandler>();
        }

        public async Task TryLogin()
        {
            await (this as ILoginProvider).Login();
        }
    }
}