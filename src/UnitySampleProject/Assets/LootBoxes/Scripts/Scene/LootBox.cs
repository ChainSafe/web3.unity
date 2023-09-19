using System;
using UnityEngine;

namespace LootBoxes.Scene
{
    public class LootBox : MonoBehaviour
    {
        public Animator animator;
        
        private bool _selected;
        public event Action<LootBox> Clicked;

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                animator.SetBool("Selected", _selected);
            }
        }

        public void PlayOpening() => animator.SetTrigger("Opening");
        public void PlayClaimingRewards() => animator.SetTrigger("ClaimingRewards");
        public void PlayDisintegrate() => animator.SetTrigger("Disintegrate");
        public void PlayCanBeClaimed() => animator.SetTrigger("CanBeClaimed");

        public void OnClick()
        {
            Clicked?.Invoke(this);
        }
    }
}