using System;
using UnityEngine;

namespace LootBoxes.Scene
{
    public class StageFocus : MonoBehaviour
    {
        private Stage stage;
        private StageCamera camera;
        
        public int FocusedItemIndex { get; private set; }
        
        public void Configure(Stage stage, StageCamera camera)
        {
            this.camera = camera;
            this.stage = stage;
        }

        public void Focus(int index)
        {
            if (index < 0 || index >= stage.StageItemCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            
            FocusedItemIndex = index;
            camera.LookAt(FocusedItemIndex);
        }

        public void FocusDelta(int delta)
        {
            var focusIndex = Mathf.Clamp(FocusedItemIndex + delta, 0, stage.StageItemCount - 1);
            Focus(focusIndex);
        }

        public void FocusImmediately(int index)
        {
            // todo implement immediate
            Focus(index);
        }
    }
}