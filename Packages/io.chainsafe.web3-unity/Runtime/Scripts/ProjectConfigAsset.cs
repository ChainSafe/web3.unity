using System.Collections.Generic;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Chains;
using UnityEngine;

namespace ChainSafe.Gaming
{
    [CreateAssetMenu(menuName = "ChainSafe/Project Configuration", fileName = "ProjectConfig", order = 0)]
    public class ProjectConfigAsset : ScriptableObject, ICompleteProjectConfig
    {
        [field: SerializeField] public string ProjectId { get; set; }
        [field: SerializeField] public bool EnableAnalytics { get; set; }
        [field: SerializeField] public List<ChainConfigEntry> ChainConfigs { get; set; } = new();

        IEnumerable<IChainConfig> IChainConfigSet.Configs => ChainConfigs; 
    }
}