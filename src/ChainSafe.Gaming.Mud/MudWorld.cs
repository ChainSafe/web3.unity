using System.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.Mud.Contracts.World;
using Nethereum.Web3;

namespace ChainSafe.Gaming.Mud
{
    public class MudWorld
    {
        private readonly WorldService worldService;

        public MudWorld(IWeb3 nethWeb3, string contractAddress)
        {
            worldService = new WorldService(nethWeb3, contractAddress);
        }

        public Task<TResult> Call<TFunction, TResult>()
            where TFunction : FunctionMessage, new()
        {
            return worldService.ContractHandler.QueryAsync<TFunction, TResult>();
        }

        public Task Send<TFunction>() // todo return value
            where TFunction : FunctionMessage, new()
        {
            return worldService.ContractHandler.SendRequestAndWaitForReceiptAsync<TFunction>();
        }
    }
}