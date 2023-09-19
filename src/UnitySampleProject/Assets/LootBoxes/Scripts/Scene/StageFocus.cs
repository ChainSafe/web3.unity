using System;
using System.Linq;
using LootBoxes.Scene.StageItems;
using UnityEngine;

namespace LootBoxes.Scene
{
    public class StageFocus : MonoBehaviour
    {
        private Stage stage;
        private StageCamera camera;
        
        public int FocusedItemIndex { get; private set; }
        public StageItem FocusedItem => stage.StageItems.Skip(FocusedItemIndex).First();
        
        public void Configure(Stage stage, StageCamera camera)
        {
            this.camera = camera;
            this.stage = stage;
        }

        public void Focus(int index, bool immediately = false)
        {
            if (index < 0 || (index >= stage.StageItemCount && index != 0))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            
            FocusedItemIndex = index;
            
            if (!immediately)
            {
                camera.LookAt(FocusedItemIndex);
            }
            else
            {
                camera.LookAtImmediately(FocusedItemIndex);
            }
        }

        public void FocusDelta(int delta)
        {
            var focusIndex = Mathf.Clamp(FocusedItemIndex + delta, 0, stage.StageItemCount - 1);
            Focus(focusIndex);
        }
    }
}