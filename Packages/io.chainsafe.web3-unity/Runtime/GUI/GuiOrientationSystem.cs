using UnityEngine;

namespace ChainSafe.Gaming.GUI
{
    public class GuiOrientationSystem : MonoBehaviour
    {
        public float AspectRatioThreshold = 1f;
        public bool Debug;
        public GuiOrientation DebugOrientationValue;

        public GuiOrientation Initial { get; private set; }

        private void Awake()
        {
            Initial = DetermineOrientation();
        }

        private GuiOrientation DetermineOrientation()
        {
            if (Debug)
            {
                return DebugOrientationValue;
            }

            var aspectRatio = (float)Screen.width / Screen.height;

            return aspectRatio > AspectRatioThreshold
                ? GuiOrientation.Landscape
                : GuiOrientation.Portrait;
        }
    }
}