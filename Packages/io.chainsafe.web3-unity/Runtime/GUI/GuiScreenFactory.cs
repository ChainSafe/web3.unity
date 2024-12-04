using System;

namespace ChainSafe.Gaming.GUI
{
    [Serializable]
    public class GuiScreenFactory
    {
        public GuiScreen LandscapePrefab;
        public GuiScreen PortraitPrefab;

        private GuiScreen singleScreen;

        public T GetSingle<T>()
        {
            if (!singleScreen)
            {
                singleScreen = BuildInternal();
                singleScreen.gameObject.SetActive(false);
            }

            return singleScreen.GetComponent<T>();
        }

        public T Build<T>()
        {
            var screen = BuildInternal();
            screen.gameObject.SetActive(false);
            return screen.GetComponent<T>();
        }

        private GuiScreen BuildInternal()
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