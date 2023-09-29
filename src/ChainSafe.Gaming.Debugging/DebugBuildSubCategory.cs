using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;

namespace ChainSafe.Gaming.Debugging
{
    public class DebugBuildSubCategory : IWeb3BuildSubCategory
    {
        private readonly IWeb3ServiceCollection services;

        public DebugBuildSubCategory(IWeb3ServiceCollection services)
        {
            this.services = services;
        }

        IWeb3ServiceCollection IWeb3BuildSubCategory.Services => this.services;
    }
}