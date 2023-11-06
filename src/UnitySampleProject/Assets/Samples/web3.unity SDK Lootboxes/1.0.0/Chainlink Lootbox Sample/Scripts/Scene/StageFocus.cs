using System;
using System.Linq;
using LootBoxes.Chainlink.Scene.StageItems;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene
{
    public class StageFocus : MonoBehaviour
    {
        private Stage stage;
        private StageCamera camera;

        public int FocusedItemIndex { get; private set; }

        public StageItem FocusedItem
        {
            get
            {
                if (stage.StageItems == null)
                {
                    return null;
                }

                if (FocusedItemIndex >= stage.StageItems.Count)
                {
                    return null;
                }

                return stage.StageItems.Skip(FocusedItemIndex).First();
            }
        }

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

            FocusedItem?.SetSpotlightActive(false);

            FocusedItemIndex = index;

            if (!immediately)
            {
                camera.LookAt(FocusedItemIndex);
            }
            else
            {
                camera.LookAtImmediately(FocusedItemIndex);
            }

            FocusedItem?.SetSpotlightActive(true);
        }

        public void FocusDelta(int delta)
        {
            var focusIndex = Mathf.Clamp(FocusedItemIndex + delta, 0, stage.StageItemCount - 1);
            Focus(focusIndex);
        }
    }
}