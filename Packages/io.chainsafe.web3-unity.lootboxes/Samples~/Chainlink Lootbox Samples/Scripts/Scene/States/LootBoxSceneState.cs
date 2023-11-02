using UnityEngine;

namespace LootBoxes.Chainlink.Scene.States
{
    public abstract class LootBoxSceneState : StateMachineBehaviour
    {
        private LootBoxScene context;
        private Animator animator;

        protected LootBoxScene Context =>
            context ??= animator.gameObject.GetComponent<LootBoxScene>();

        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.animator = animator;
            OnLootBoxSceneStateEnter();
        }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnLootBoxSceneStateExit();
        }

        public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnLootBoxSceneStateUpdate();
        }

        protected virtual void OnLootBoxSceneStateEnter()
        {
        }

        protected virtual void OnLootBoxSceneStateExit()
        {
        }

        protected virtual void OnLootBoxSceneStateUpdate()
        {
        }
    }
}