using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage
{
    [CreateAssetMenu(fileName = "HyperPlayConfigData", menuName = 
        "ScriptableObjects/HyperPlayConfigScriptableObject", order = 2)]
    public class HyperPlayConfigScriptableObject : ScriptableObject
    {
        [SerializeField] private bool storedSessionAvailable;
        [SerializeField] private string storedWallet;
        [SerializeField] private bool rememberMe;
        
        public bool RememberMe
        {
            get => rememberMe;
            set => rememberMe = value;
        }
        
        public bool StoredSessionAvailable
        {
            get => storedSessionAvailable;
            set => storedSessionAvailable = value;
        }

        public string StoredWallet
        {
            get => storedWallet;
            set => storedWallet = value;
        }
    }
}