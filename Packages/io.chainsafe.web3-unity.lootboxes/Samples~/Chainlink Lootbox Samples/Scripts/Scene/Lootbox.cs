using System;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene
{
    public class Lootbox : MonoBehaviour
    {
        public Animator animator;
        public ParticleSystem claimedFx;

        private bool _selected;
        public event Action<Lootbox> Clicked;

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                animator.SetBool("Float", _selected);
            }
        }

        public void PlayOpening()
        {
            animator.SetTrigger("Opening");
            animator.SetBool("Float", true);
        }

        public void PlayReadyToBeClaimed()
        {
            animator.SetTrigger("ReadyToBeClaimed");
            animator.SetBool("Float", true);
        }

        public void PlayClaimingRewards()
        {
            claimedFx.Play();
        }

        public void PlayDisintegrate() => animator.SetTrigger("Disintegrate");

        public void OnClick()
        {
            Clicked?.Invoke(this);
        }
    }
}