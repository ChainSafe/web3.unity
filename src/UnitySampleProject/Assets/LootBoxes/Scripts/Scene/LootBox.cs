using UnityEngine;

namespace LootBoxes.Scene
{
    public class LootBox : MonoBehaviour
    {
        public Animator animator;
        
        private bool _selected;

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                animator.SetBool("Selected", _selected);
            }
        }

        public void PlayDisintegrate() => animator.SetTrigger("Disintegrate");

        public void PlayOpenInitiated() => animator.SetTrigger("OpenInitiated");

        public void OnClick()
        {
            Selected = !Selected;
        }
    }
}