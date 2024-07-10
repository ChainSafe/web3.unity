using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainSafe.Gaming
{
    [CreateAssetMenu(menuName = "ChainSafe/Connection Provider Config", fileName = "ConnectionProviderConfig")]
    public class ConnectionProviderConfig : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        
        [field: SerializeField] public ConnectionProvider ProviderRow { get; private set; }
    }
}
