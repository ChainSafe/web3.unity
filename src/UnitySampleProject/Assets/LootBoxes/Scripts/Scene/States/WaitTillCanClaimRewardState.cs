using System.Threading;
using System.Threading.Tasks;

namespace LootBoxes.Scene.States
{
    public class WaitTillCanClaimRewardState : LootBoxSceneState
    {
        public float pollInterval = 1f;

        private readonly CancellationTokenSource cancellationSource = new();
        
        protected override async void OnLootBoxSceneStateEnter()
        {
            try
            {
                await PollTillCanClaimReward(cancellationSource.Token);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            Context.animator.SetTrigger("CanClaimRewards");
        }

        protected override void OnLootBoxSceneStateExit()
        {
            cancellationSource.Cancel();
        }

        private async Task PollTillCanClaimReward(CancellationToken cancellationToken)
        {
            while (!await Context.CanClaimRewards())
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                await SafeDelay.WaitForSeconds(pollInterval);
            }
        }
    }
}