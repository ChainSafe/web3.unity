using UnityEngine;
using UnityEngine.Events;

namespace ChainSafe.Gaming.GUI
{
    public class GuiManager : MonoBehaviour
    {
        private const string ResourceName = "ChainSafe GUI Manager";
        private static GuiManager CachedInstance;

        public static GuiManager Instance => CachedInstance ?? (CachedInstance = LoadPrefab());

        private static GuiManager LoadPrefab()
        {
            var prefab = Resources.Load<GuiManager>(ResourceName);
            var manager = Instantiate(prefab);
            manager.gameObject.hideFlags = HideFlags.HideInHierarchy;
            DontDestroyOnLoad(manager);
            return manager;
        }

        [field:SerializeField] public GuiOrientationSystem Orientation { get; private set; }
        [field:SerializeField] public GuiOverlayManager Overlays { get; private set; }
        
        public GuiCoordinationSystem Coordination { get; } = new();
    }
}