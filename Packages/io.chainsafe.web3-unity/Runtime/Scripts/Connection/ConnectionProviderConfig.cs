using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Connection Provider Config Scriptable Object.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/Connection Provider Config", fileName = "ConnectionProviderConfig")]
    public class ConnectionProviderConfig : ScriptableObject
    {
        /// <summary>
        /// Name of connection provider.
        /// </summary>
        [field: SerializeField] public string Name { get; private set; }

        /// <summary>
        /// Row Prefab of connection provider.
        /// </summary>
        [field: SerializeField] public ConnectionProvider ProviderRow { get; private set; }
    }
}
