using System;
using System.Collections.Generic;
using System.Text;

namespace ChainSafe.GamingSdk.EVM.Unity
{
    internal class EnsureUnityBuild
    {
        // Note: to build this library, the 'Unity' build property must be set.
        // You can use build.sh, publish.sh and publish-to-unity.sh to build this code.
#pragma warning disable CS0169
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "CodeQuality",
            "IDE0051:Remove unused private members",
            Justification = "Included as a check, not meant to be used")]
        private Web3Unity.Scripts.Library.Ethers.Utils.IsUnityBuild isUnityBuild;
#pragma warning restore CS0169
    }
}
