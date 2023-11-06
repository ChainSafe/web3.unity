using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene.States
{
    public class WaitTillCanClaimRewardState : LootBoxSceneState
    {
        public float pollInterval = 1f;

        private CancellationTokenSource cancellationSource;

        protected override async void OnLootBoxSceneStateEnter()
        {
            Context.openLootBoxUI.gameObject.SetActive(true);

            cancellationSource = new CancellationTokenSource();
            try
            {
                await PollTillCanClaimReward(cancellationSource.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            Context.animator.SetTrigger("CanClaimRewards");
        }

        protected override void OnLootBoxSceneStateExit()
        {
            Context.openLootBoxUI.gameObject.SetActive(false);

            cancellationSource.Cancel();
        }

        private async Task PollTillCanClaimReward(CancellationToken cancellationToken)
        {
            while (!await Context.CanClaimRewards())
            {
                cancellationToken.ThrowIfCancellationRequested();

                await WaitForSeconds(pollInterval);
            }
        }

        private async Task WaitForSeconds(float seconds)
        {
            // Task.Delay doesn't work on WebGL
#if UNITY_WEBGL && !UNITY_EDITOR
            var now = Time.time;
            while (Time.time - now < seconds) await Task.Yield();
#else
            await Task.Delay((int)(seconds * 1000));
#endif
        }
    }
}