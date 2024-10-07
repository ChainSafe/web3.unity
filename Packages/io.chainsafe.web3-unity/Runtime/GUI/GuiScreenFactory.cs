using System;

namespace ChainSafe.Gaming.GUI
{
    [Serializable]
    public class GuiScreenFactory
    {
        public GuiScreen LandscapePrefab;
        public GuiScreen PortraitPrefab;

        private GuiScreen builtScreen;

        public T GetSingle<T>()
        {
            if (!builtScreen)
            {
                builtScreen = Build();
                builtScreen.gameObject.SetActive(false);
            }
            
            return builtScreen.GetComponent<T>();
        }

        private GuiScreen Build()
        {
            var prefab = FetchPrefab();
            var screen = UnityEngine.Object.Instantiate(prefab);
            screen.gameObject.name = prefab.gameObject.name;
            return screen;

            GuiScreen FetchPrefab()
            {
                switch (GuiManager.Instance.Orientation.Initial)
                {
                    case GuiOrientation.Landscape:
                        return LandscapePrefab;
                    case GuiOrientation.Portrait:
                        return PortraitPrefab;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}