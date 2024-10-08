using System.Collections.Generic;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Chains;
using UnityEngine;

namespace ChainSafe.Gaming
{
    [CreateAssetMenu(menuName = "ChainSafe/Project Configuration", fileName = "Web3Config", order = -100)]
    public class Web3ConfigAsset : ScriptableObject, ICompleteProjectConfig
    {
        [field: SerializeField] public string ProjectId { get; set; }
        [field: SerializeField] public bool EnableAnalytics { get; set; } = true;
        [field: SerializeField] public List<ChainConfigEntry> ChainConfigs { get; set; } = new();

        IEnumerable<IChainConfig> IChainConfigSet.Configs => ChainConfigs;
    }
}