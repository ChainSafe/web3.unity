using Prefabs.Web3AuthWallet.Interfaces;

namespace Prefabs.Web3AuthWallet.Utils
{
    public class TransactionConfig : ITransactionConfig
    {
        private ProjectConfigScriptableObject projectConfigSo;

        public TransactionConfig(ProjectConfigScriptableObject projectConfigSo)
        {
            this.projectConfigSo = projectConfigSo;
        }

        public string GetProjectId() => projectConfigSo.ProjectId;

        public string GetChain() => projectConfigSo.Chain;

        public string GetNetwork() => projectConfigSo.Network;

        public string GetRpc() => projectConfigSo.Rpc;
    }
}