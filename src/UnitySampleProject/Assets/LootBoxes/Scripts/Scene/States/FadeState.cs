using System;
using System.Threading.Tasks;

namespace LootBoxes.Scene.States
{
    public enum FadeGoal
    {
        FadeIn,
        FadeOut
    }
    
    public class FadeState : LootBoxSceneState
    {
        public FadeGoal Goal = FadeGoal.FadeIn;
        public bool Await = true;
        
        protected override async void OnLootBoxSceneStateEnter()
        {
            var task = LaunchFade();
            if (Await)
            {
                await task;
            }
            
            Context.animator.SetTrigger("Next");
        }

        private Task LaunchFade()
        {
            return Goal switch
            {
                FadeGoal.FadeIn => Context.blackout.FadeIn(),
                FadeGoal.FadeOut => Context.blackout.FadeOut(),
                _ => throw new ArgumentOutOfRangeException(nameof(Goal), Goal, null)
            };
        }
    }
}